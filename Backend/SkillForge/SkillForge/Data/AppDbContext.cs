using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SkillForge.Models.Database;

namespace SkillForge.Data;

public class AppDbContext : DbContext
{
    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<AdminRole> AdminRoles { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleApproval> ArticleApprovals { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<ArticleRating> ArticleRatings { get; set; }
    public DbSet<ArticleView> ArticleViews { get; set; }
    public DbSet<GuestArticleView> GuestArticleViews { get; set; }
    public DbSet<RegisteredArticleView> RegisteredArticleViews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentRating> CommentRatings { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    private void SetTimestamps()
    {
        DateTime now = DateTime.Now;

        foreach (EntityEntry changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is BaseEntity baseEntity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = now;
                        baseEntity.UpdatedAt = now;
                        break;

                    case EntityState.Modified:
                        Entry(baseEntity).Property(x => x.CreatedAt).IsModified = false;
                        baseEntity.UpdatedAt = now;
                        break;
                }
            }
        }
    }

    public override int SaveChanges()
    {
        SetTimestamps();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();

        return base.SaveChangesAsync(cancellationToken);
    }
}
