using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Services;

public interface IFrontendService
{
    ArticlePageData CreateArticlePageData(Article article);

    ArticleCard CreateArticleCard(Article article);

    ArticleSearchItem CreateArticleSearchItem(Article article);

    TopArticleItem CreateTopArticleItem(Article article);

    UserInfo GetUserInfo(User user);

    UserLink CreateUserLink(User user);

    TagLink CreateTagLink(Tag tag);

    CommentModel CreateCommentModel(Comment comment);
}