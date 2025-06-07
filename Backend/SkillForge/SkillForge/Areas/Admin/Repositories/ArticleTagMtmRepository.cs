using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class ArticleTagMtmRepository : ManyToManyRepository<ArticleTag>, IArticleTagMtmRepository
{
    protected override DbSet<ArticleTag> DbSet => db.ArticleTags;

    public ArticleTagMtmRepository(AppDbContext db) : base(db)
    {
    }
}
