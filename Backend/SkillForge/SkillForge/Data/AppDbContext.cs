using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SkillForge.Data.Seeders;
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
    public DbSet<FavoriteArticle> FavoriteArticles { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<TagFollow> TagFollows { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ArticleReport> ArticleReports { get; set; }
    public DbSet<CommentReport> CommentReports { get; set; }
    public DbSet<UserReport> UserReports { get; set; }
    public DbSet<AccountSuspension> AccountSuspensions { get; set; }

    private readonly IAdminRoleSeeder adminRoleSeeder;

    public AppDbContext(DbContextOptions<AppDbContext> options, IAdminRoleSeeder adminRoleSeeder)
        : base(options)
    {
        this.adminRoleSeeder = adminRoleSeeder;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        adminRoleSeeder.Seed(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(e => e.Followers)
            .WithOne(e => e.FollowedUser);

        modelBuilder.Entity<User>()
            .HasMany(e => e.Followings)
            .WithOne(e => e.Follower);

        modelBuilder.Entity<User>()
            .HasIndex(nameof(User.Name))
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(nameof(User.Email))
            .IsUnique();

        modelBuilder.Entity<UserFollow>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_UserFollows_NoSelfReference",
                nameof(UserFollow.FollowerId) + " <> " + nameof(UserFollow.FollowedUserId)));

        modelBuilder.Entity<UserFollow>()
            .HasIndex(nameof(UserFollow.FollowerId), nameof(UserFollow.FollowedUserId))
            .IsUnique();

        modelBuilder.Entity<TagFollow>()
            .HasIndex(nameof(TagFollow.UserId), nameof(TagFollow.TagId))
            .IsUnique();

        modelBuilder.Entity<CommentRating>()
            .HasIndex(nameof(CommentRating.UserId), nameof(CommentRating.CommentId))
            .IsUnique();

        modelBuilder.Entity<ArticleTag>()
            .HasIndex(nameof(ArticleTag.ArticleId), nameof(ArticleTag.TagId))
            .IsUnique();

        modelBuilder.Entity<FavoriteArticle>()
            .HasIndex(nameof(FavoriteArticle.UserId), nameof(FavoriteArticle.ArticleId))
            .IsUnique();

        modelBuilder.Entity<Category>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Categories_NoSelfReference",
                nameof(Category.Id) + " <> " + nameof(Category.ParentId)));

        modelBuilder.Entity<AdminUser>()
            .HasIndex(nameof(AdminUser.Name))
            .IsUnique();

        modelBuilder.Entity<AdminUser>()
            .HasIndex(nameof(AdminUser.Email))
            .IsUnique();

        modelBuilder.Entity<AdminRole>()
            .HasIndex(nameof(AdminRole.Code))
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(nameof(Category.Code))
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(nameof(Tag.Name))
            .IsUnique();
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
