using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SkillForge.Areas.Admin.Models.ManyToMany;
using SkillForge.Attributes;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public abstract class ManyToManyRepository<TManyToManyEntity, TUpdateModel> : IManyToManyRepository<TManyToManyEntity, TUpdateModel>
    where TManyToManyEntity : BaseEntity
    where TUpdateModel : MtmUpdate, new()
{
    protected abstract DbSet<TManyToManyEntity> DbSet { get; }

    protected readonly AppDbContext db;

    public ManyToManyRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<List<TManyToManyEntity>> PrepareSide(List<TUpdateModel> updates, bool leftSide)
    {
        List<TManyToManyEntity> result = new();

        foreach (TUpdateModel update in updates)
        {
            TManyToManyEntity relationship = await CreateNewRelationshipPrivate(0, update, leftSide);
            result.Add(relationship);
        }

        return result;
    }

    public async Task<List<TManyToManyEntity>> PrepareLeft(List<TUpdateModel> updates)
    {
        return await PrepareSide(updates, leftSide: true);
    }

    public async Task<List<TManyToManyEntity>> PrepareRight(List<TUpdateModel> updates)
    {
        return await PrepareSide(updates, leftSide: false);
    }

    public async Task UpdateSide(int id, List<TUpdateModel> updates, bool leftSide)
    {
        List<TManyToManyEntity> related = await OnQuery(DbSet)
            .Where(GenerateSideFilter(id, leftSide))
            .ToListAsync();

        List<int> otherSideIds = updates.ConvertAll(e => e.Id);

        List<TManyToManyEntity> toDelete = await DbSet
            .Where(GenerateSideFilter(id, leftSide))
            .Where(GenerateContainsFilter(otherSideIds, leftSide: !leftSide, not: true))
            .ToListAsync();

        DbSet.RemoveRange(toDelete);

        List<TManyToManyEntity> toInsert = new();
        List<TManyToManyEntity> toUpdate = new();

        foreach (TUpdateModel update in updates)
        {
            TManyToManyEntity? match = related.Where(e => GetForeignKey(e, !leftSide) == update.Id).FirstOrDefault();

            if (match == null)
            {
                TManyToManyEntity relationship = await CreateNewRelationshipPrivate(id, update, leftSide);
                toInsert.Add(relationship);
            }
            else
            {
                TManyToManyEntity relationship = await UpdateRelationshipAsync(match, update);

                if (relationship != null)
                {
                    toUpdate.Add(relationship);
                }
            }
        }

        DbSet.AddRange(toInsert);
        DbSet.UpdateRange(toUpdate);

        await db.SaveChangesAsync();
    }

    public Task UpdateSide(int id, List<int> updatedOtherSideIds, bool leftSide)
    {
        List<TUpdateModel> updates = new();

        foreach (int otherSideId in updatedOtherSideIds)
        {
            updates.Add(new TUpdateModel { Id = otherSideId });
        }

        return UpdateSide(id, updates, leftSide);
    }

    public Task UpdateLeft(int id, List<int> updatedRightSideIds)
    {
        return UpdateSide(id, updatedRightSideIds, true);
    }

    public Task UpdateRight(int id, List<int> updatedLeftSideIds)
    {
        return UpdateSide(id, updatedLeftSideIds, false);
    }

    public Task UpdateLeft(int id, List<TUpdateModel> updates)
    {
        return UpdateSide(id, updates, true);
    }

    public Task UpdateRight(int id, List<TUpdateModel> updates)
    {
        return UpdateSide(id, updates, false);
    }

    public virtual IQueryable<TManyToManyEntity> OnQuery(IQueryable<TManyToManyEntity> query)
    {
        return query;
    }

    private async Task<TManyToManyEntity> CreateNewRelationshipPrivate(int id, TUpdateModel update, bool leftSide)
    {
        TManyToManyEntity relationship = await CreateNewRelationshipAsync(update);
        GetSidePropertyOrThrow(leftSide).SetValue(relationship, id);
        GetSidePropertyOrThrow(!leftSide).SetValue(relationship, update.Id);

        return relationship;
    }

    public virtual TManyToManyEntity CreateNewRelationship(TUpdateModel update)
    {
        return Activator.CreateInstance<TManyToManyEntity>();
    }

    public virtual TManyToManyEntity UpdateRelationship(TManyToManyEntity relationship, TUpdateModel update)
    {
        return relationship;
    }

    public virtual Task<TManyToManyEntity> CreateNewRelationshipAsync(TUpdateModel update)
    {
        return Task.FromResult(CreateNewRelationship(update));
    }

    public virtual Task<TManyToManyEntity> UpdateRelationshipAsync(TManyToManyEntity relationship, TUpdateModel update)
    {
        return Task.FromResult(UpdateRelationship(relationship, update));
    }

    public int GetForeignKey(TManyToManyEntity e, bool leftSide)
    {
        PropertyInfo prop = GetSidePropertyOrThrow(leftSide);
        object value = prop.GetValue(e)
            ?? throw new Exception($"The property representing the foreign key value is unexpectedly null.");

        return (int)value;
    }

    public PropertyInfo GetSidePropertyOrThrow(bool leftSide)
    {
        ManyToManyEntityAttribute attr = GetManyToManyAttributeOrThrow();
        string foreignKeyName = leftSide ? attr.leftSide : attr.rightSide;
        PropertyInfo? prop = typeof(TManyToManyEntity).GetProperty(foreignKeyName);

        if (prop == null)
        {
            throw new Exception(
                $"The property {foreignKeyName} specified in the many-to-many relationship " +
                $"was not found in the many-to-many entity {typeof(TManyToManyEntity).ShortDisplayName()}");
        }

        return prop;
    }

    public ManyToManyEntityAttribute GetManyToManyAttributeOrThrow()
    {
        return typeof(TManyToManyEntity).GetCustomAttribute<ManyToManyEntityAttribute>()
            ?? throw new Exception("Many-to-many entity attribute not specified!");
    }

    public Expression<Func<TManyToManyEntity, bool>> GenerateSideExpression(bool leftSide, Func<MemberExpression, Expression> callback)
    {
        ParameterExpression param = Expression.Parameter(typeof(TManyToManyEntity), "x");

        ManyToManyEntityAttribute attr = GetManyToManyAttributeOrThrow();
        string foreignKeyName = leftSide ? attr.leftSide : attr.rightSide;
        MemberExpression foreignKeyProperty = Expression.Property(param, foreignKeyName);

        Expression body = callback.Invoke(foreignKeyProperty);

        return Expression.Lambda<Func<TManyToManyEntity, bool>>(body, param);
    }

    public Expression<Func<TManyToManyEntity, bool>> GenerateSideFilter(int id, bool leftSide)
    {
        return GenerateSideExpression(leftSide, fk =>
        {
            return Expression.Equal(fk, Expression.Constant(id));
        });
    }

    public Expression<Func<TManyToManyEntity, bool>> GenerateContainsFilter(List<int> ids, bool leftSide, bool not)
    {
        return GenerateSideExpression(leftSide, fk =>
        {
            ConstantExpression constant = Expression.Constant(ids);
            MethodCallExpression containsExpr = Expression.Call(constant, "Contains", null, fk);

            if (not)
            {
                return Expression.Not(containsExpr);
            }

            return containsExpr;
        });
    }
}

public abstract class ManyToManyRepository<TManyToManyEntity> : ManyToManyRepository<TManyToManyEntity, MtmUpdate>
    where TManyToManyEntity : BaseEntity
{
    protected ManyToManyRepository(AppDbContext db)
        : base(db)
    {
    }
}