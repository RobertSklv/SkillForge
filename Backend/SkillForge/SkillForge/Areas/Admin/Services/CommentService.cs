using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Rating;
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

    public async Task Rate(int userId, int commentId, UserRatingData rate)
    {
        CommentRating? rating = await GetUserRating(userId, commentId);

        short oldRate = 0;

        if (rating != null)
        {
            oldRate = rating.Rate;
        }

        rating ??= new CommentRating()
        {
            UserId = userId,
            CommentId = commentId,
        };

        rating.Rate = Math.Clamp(rate.Rate, (short)-1, (short)1);

        Comment article = await GetStrict(commentId);

        if (rating.Rate != oldRate)
        {
            if (oldRate > 0)
            {
                article.ThumbsUp--;
            }
            else if (oldRate < 0)
            {
                article.ThumbsDown--;
            }

            if (rating.Rate > 0)
            {
                article.ThumbsUp++;
            }
            else if (rating.Rate < 0)
            {
                article.ThumbsDown++;
            }
        }

        await repository.UpsertUserRating(rating);
    }

    public Task<CommentRating?> GetUserRating(int userId, int commentId)
    {
        return repository.GetUserRating(userId, commentId);
    }

    public CommentModel CreateCommentModel(Comment comment)
    {
        return new CommentModel
        {
            CommentId = comment.Id,
            User = new UserLink
            {
                Id = comment.User!.Id,
                Name = comment.User.Name,
                AvatarImage = comment.User.AvatarPath,
            },
            Content = comment.Content,
            DateWritten = (DateTime)comment.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = comment.ThumbsUp,
                ThumbsDown = comment.ThumbsDown,
            },
        };
    }
}
