using Hangfire;
using SkillForge.Cron;

namespace SkillForge.Configuration;

public static class CronConfiguration
{
    public static void Setup()
    {
        RecurringJob.AddOrUpdate<IAggregateArticleRatingJob>(
            "aggregate-article-rating",
            job => job.RunAsync(),
            "30 * * * *");

        RecurringJob.AddOrUpdate<IAggregateArticleViewJob>(
            "aggregate-article-views",
            job => job.RunAsync(),
            "0 */2 * * *");

        RecurringJob.AddOrUpdate<IAggregateCommentRatingJob>(
            "aggregate-comment-rating",
            job => job.RunAsync(),
            "5 */2 * * *");

        RecurringJob.AddOrUpdate<IAggregateTagArticleJob>(
            "aggregate-tag-article-count",
            job => job.RunAsync(),
            "10 */3 * * *");

        RecurringJob.AddOrUpdate<IAggregateTagFollowJob>(
            "aggregate-tag-follows",
            job => job.RunAsync(),
            "15 */3 * * *");

        RecurringJob.AddOrUpdate<IAggregateUserArticlesJob>(
            "aggregate-user-article-count",
            job => job.RunAsync(),
            "20 */3 * * *");

        RecurringJob.AddOrUpdate<IAggregateUserFollowJob>(
            "aggregate-user-follows",
            job => job.RunAsync(),
            "25 */3 * * *");

        RecurringJob.AddOrUpdate<IGenerateXmlSitemapJob>(
            "generate-xml-sitemap",
            job => job.RunAsync(),
            "0 0 * * *");
    }
}
