using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkillForge.Configuration;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.BackgroundTasks;

public class GenerateXmlSitemapService : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IOptions<SiteOptions> siteOptions;
    private readonly IWebHostEnvironment webHostEnvironment;

    public GenerateXmlSitemapService(IServiceScopeFactory scopeFactory, IOptions<SiteOptions> siteOptions, IWebHostEnvironment webHostEnvironment)
    {
        this.scopeFactory = scopeFactory;
        this.siteOptions = siteOptions;
        this.webHostEnvironment = webHostEnvironment;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        using PeriodicTimer timer = new(TimeSpan.FromDays(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await Generate(db);
        }
    }

    public async Task Generate(AppDbContext db)
    {
        List<User> users = await db.Users.ToListAsync();
        List<Article> articles = await db.Articles.ToListAsync();
        List<Tag> tags = await db.Tags.ToListAsync();

        (XmlDocument document, XmlElement root) = CreateXmlSitemap("sitemapindex");

        string sitemapUsersFilename = GenerateUserSitemap(users, siteOptions.Value.FrontendUrl);
        string sitemapArticlesFilename = GenerateArticleSitemap(articles, siteOptions.Value.FrontendUrl);
        string sitemapTagsFilename = GenerateTagSitemap(tags, siteOptions.Value.FrontendUrl);

        XmlElement sitemapUsers = CreateSitemapElement(document, string.Join('/', siteOptions.Value.BackendUrl, sitemapUsersFilename));
        root.AppendChild(sitemapUsers);

        XmlElement sitemapArticles = CreateSitemapElement(document, string.Join('/', siteOptions.Value.BackendUrl, sitemapArticlesFilename));
        root.AppendChild(sitemapArticles);

        XmlElement sitemapTags = CreateSitemapElement(document, string.Join('/', siteOptions.Value.BackendUrl, sitemapTagsFilename));
        root.AppendChild(sitemapTags);

        document.Save(Path.Combine(webHostEnvironment.WebRootPath, "sitemap-index.xml"));
    }

    public string GenerateUserSitemap(List<User> users, string baseUrl)
    {
        (XmlDocument document, XmlElement root) = CreateXmlSitemap("urlset");

        foreach (User user in users)
        {
            XmlElement url = CreateUrl(document, string.Join('/', baseUrl, "user", user.Name), user.UpdatedAt, "daily");
            root.AppendChild(url);
        }

        string filename = "sitemap-users.xml";

        document.Save(Path.Combine(webHostEnvironment.WebRootPath, filename));

        return filename;
    }

    public string GenerateArticleSitemap(List<Article> articles, string baseUrl)
    {
        (XmlDocument document, XmlElement root) = CreateXmlSitemap("urlset");

        foreach (Article article in articles)
        {
            XmlElement url = CreateUrl(document, string.Join('/', baseUrl, "article", article.Id), article.UpdatedAt, "daily");
            root.AppendChild(url);
        }

        string filename = "sitemap-articles.xml";

        document.Save(Path.Combine(webHostEnvironment.WebRootPath, filename));

        return filename;
    }

    public string GenerateTagSitemap(List<Tag> tags, string baseUrl)
    {
        (XmlDocument document, XmlElement root) = CreateXmlSitemap("urlset");

        foreach (Tag tag in tags)
        {
            XmlElement url = CreateUrl(document, string.Join('/', baseUrl, "tag", tag.Name), tag.UpdatedAt, "daily");
            root.AppendChild(url);
        }

        string filename = "sitemap-tags.xml";

        document.Save(Path.Combine(webHostEnvironment.WebRootPath, filename));

        return filename;
    }

    public XmlElement CreateSitemapElement(XmlDocument document, string loc)
    {
        XmlElement sitemap = document.CreateElement("sitemap");

        sitemap.AppendChild(CreateProperty(document, "loc", loc));

        return sitemap;
    }

    public XmlElement CreateUrl(XmlDocument document, string loc, DateTime? lastmod, string changefreq)
    {
        XmlElement url = document.CreateElement("url");

        url.AppendChild(CreateProperty(document, "loc", loc));
        url.AppendChild(CreateProperty(document, "lastmod", $"{lastmod:yyyy-MM-dd}"));
        url.AppendChild(CreateProperty(document, "changefreq", changefreq));

        return url;
    }

    public XmlElement CreateProperty(XmlDocument document, string name, string value)
    {
        XmlElement prop = document.CreateElement(name);
        prop.InnerText = value;

        return prop;
    }

    public (XmlDocument document, XmlElement root) CreateXmlSitemap(string rootElementName)
    {
        XmlDocument document = new();
        document.CreateXmlDeclaration("1.0", "utf-8", null);

        XmlElement root = document.CreateElement(rootElementName);
        document.AppendChild(root);
        root.SetAttribute("xmlns", "https://www.sitemaps.org/schemas/sitemap/0.9");

        return (document, root);
    }
}
