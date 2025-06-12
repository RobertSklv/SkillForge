using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Services;

public class FrontendService : IFrontendService
{
    public ArticleCard CreateArticleCard(Article article)
    {
        return new ArticleCard()
        {
            Author = CreateUserLink(article.Author!),
            ArticleId = article.Id,
            Title = article.Title,
            CoverImage = article.Image,
            CategoryCode = article.Category!.Code,
            CategoryName = article.Category.DisplayedName,
            DatePublished = (DateTime)article.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = article.ThumbsUp,
                ThumbsDown = article.ThumbsDown,
                UserRating = 0,
            },
            Comments = article.Comments?
                .OrderBy(c => c.CreatedAt)
                .Take(2)
                .ToList()
                .ConvertAll(CreateCommentModel)
                ?? new(),
            TotalComments = article.Comments?.Count ?? 0,
            Tags = article.Tags?.ConvertAll(at => CreateTagLink(at.Tag!)) ?? new()
        };
    }

    public ArticlePageData CreateArticlePageData(Article article)
    {
        return new ArticlePageData
        {
            ArticleId = article.Id,
            Author = CreateUserLink(article.Author!),
            Title = article.Title,
            CategoryCode = article.Category!.Code,
            CategoryName = article.Category.DisplayedName,
            Content = article.Content,
            CoverImage = article.Image,
            Tags = article.Tags!.ConvertAll(t => t.Tag!.Name).ToList(),
            DatePublished = (DateTime)article.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = article.ThumbsUp,
                ThumbsDown = article.ThumbsDown,
            },
            Views = article.ViewCount,
            Comments = article.Comments!.ConvertAll(CreateCommentModel)
        };
    }

    public TopArticleItem CreateTopArticleItem(Article article)
    {
        return new TopArticleItem()
        {
            Author = CreateUserLink(article.Author!),
            ArticleId = article.Id,
            Title = article.Title,
            ViewCount = article.ViewCount,
            CommentCount = article.Comments!.Count,
            DatePublished = (DateTime)article.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = article.ThumbsUp,
                ThumbsDown = article.ThumbsDown,
                UserRating = 0,
            }
        };
    }

    public ArticleSearchItem CreateArticleSearchItem(Article article)
    {
        return new ArticleSearchItem
        {
            ArticleId = article.Id,
            Title = article.Title,
            AuthorName = article.Author!.Name,
            DatePosted = (DateTime)article.CreatedAt!,
        };
    }

    public UserInfo GetUserInfo(User user)
    {
        return new UserInfo
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            AvatarPath = user.AvatarPath
        };
    }

    public UserLink CreateUserLink(User user)
    {
        return new UserLink
        {
            Name = user.Name,
            AvatarImage = user.AvatarPath,
        };
    }

    public TagLink CreateTagLink(Tag tag)
    {
        return new TagLink
        {
            Name = tag.Name,
            Description = tag.Description,
        };
    }

    public CommentModel CreateCommentModel(Comment comment)
    {
        return new CommentModel
        {
            CommentId = comment.Id,
            User = CreateUserLink(comment.User!),
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
