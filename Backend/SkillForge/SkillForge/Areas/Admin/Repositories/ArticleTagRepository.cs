using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class ArticleTagRepository : ManyToManyRepository<ArticleTag>, IArticleTagRepository
{
    protected override DbSet<ArticleTag> DbSet => db.ArticleTags;

    public ArticleTagRepository(AppDbContext db) : base(db)
    {
    }
}
