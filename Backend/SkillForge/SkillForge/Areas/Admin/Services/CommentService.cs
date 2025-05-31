using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class CommentService : CrudService<Comment>, ICommentService
{
    private readonly ICommentRepository repository;

    public CommentService(ICommentRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }

    public async Task<Comment> Add(int userId, int articleId, string content)
    {
        Comment comment = new()
        {
            UserId = userId,
            ArticleId = articleId,
            Content = content,
        };

        await Upsert(comment);

        return comment;
    }
}
