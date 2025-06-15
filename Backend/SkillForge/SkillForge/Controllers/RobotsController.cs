using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkillForge.Configuration;

namespace SkillForge.Controllers;

public class RobotsController : Controller
{
    private readonly IOptions<SiteOptions> siteOptions;

    public RobotsController(IOptions<SiteOptions> siteOptions)
    {
        this.siteOptions = siteOptions;
    }

    [Route("/robots.txt")]
    public IActionResult Robots()
    {
        string baseUrl = siteOptions.Value.BackendUrl.TrimEnd('/');
        string sitemapUrl = $"{baseUrl}/sitemap-index.xml";

        string content = "User-agent: *" +
            "\nDisallow: /Admin/" +
            "\nDisallow: /join" +
            "\nDisallow: /article/*/edit" +
            "\nDisallow: /article/create" +
            "\nDisallow: /account/" +
            "\nDisallow: /search" +
            "\n\nSitemap: " + sitemapUrl;

        return new ContentResult
        {
            Content = content,
            ContentType = "text/plain",
            StatusCode = 200
        };
    }
}
