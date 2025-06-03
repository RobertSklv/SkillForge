using System.Linq.Expressions;
using System.Reflection;
using SkillForge.Areas.Admin.Models.ManyToMany;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IManyToManyRepository<TManyToManyEntity, TUpdateModel>
    where TManyToManyEntity : BaseEntity
    where TUpdateModel : MtmUpdate, new()
{
    Task<List<TManyToManyEntity>> PrepareSide(List<TUpdateModel> updates, bool leftSide);

    Task<List<TManyToManyEntity>> PrepareLeft(List<TUpdateModel> updates);

    Task<List<TManyToManyEntity>> PrepareRight(List<TUpdateModel> updates);

    Task UpdateSide(int id, List<TUpdateModel> updates, bool leftSide);

    Task UpdateSide(int id, List<int> updatedOtherSideIds, bool leftSide);

    Task UpdateLeft(int id, List<int> updatedRightSideIds);

    Task UpdateRight(int id, List<int> updatedLeftSideIds);

    Task UpdateLeft(int id, List<TUpdateModel> updates);

    Task UpdateRight(int id, List<TUpdateModel> updates);

    TManyToManyEntity CreateNewRelationship(TUpdateModel update);

    TManyToManyEntity UpdateRelationship(TManyToManyEntity relationship, TUpdateModel update);

    Task<TManyToManyEntity> CreateNewRelationshipAsync(TUpdateModel update);

    Task<TManyToManyEntity> UpdateRelationshipAsync(TManyToManyEntity relationship, TUpdateModel update);

    int GetForeignKey(TManyToManyEntity e, bool leftSide);

    PropertyInfo GetSidePropertyOrThrow(bool leftSide);

    ManyToManyEntityAttribute GetManyToManyAttributeOrThrow();

    Expression<Func<TManyToManyEntity, bool>> GenerateSideExpression(bool leftSide, Func<MemberExpression, Expression> callback);

    Expression<Func<TManyToManyEntity, bool>> GenerateSideFilter(int id, bool leftSide);

    Expression<Func<TManyToManyEntity, bool>> GenerateContainsFilter(List<int> ids, bool leftSide, bool not);
}

public interface IManyToManyRepository<TManyToManyEntity> : IManyToManyRepository<TManyToManyEntity, MtmUpdate>
    where TManyToManyEntity : BaseEntity
{
}