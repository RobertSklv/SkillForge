using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IArticleTagMtmRepository : IManyToManyRepository<ArticleTag>
{
}
