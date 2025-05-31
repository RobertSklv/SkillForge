using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface ICommentService : ICrudService<Comment>
{
    Task<Comment> Add(int userId, int articleId, string content);
}
