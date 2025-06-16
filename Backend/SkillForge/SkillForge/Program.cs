using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Areas.Admin.Services;
using SkillForge.BackgroundTasks;
using SkillForge.Configuration;
using SkillForge.Data;
using SkillForge.Data.Seeders;
using SkillForge.Middleware;
using SkillForge.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, p => p.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

//Admin Cookie Authentication
builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>

    {
        options.Cookie.Name = "Auth";
        options.LoginPath = "/Admin/Login";
    });

//Admin Panel Installation Authentication
builder.Services.AddAuthentication()
    .AddCookie("AdminInstallCookie", options =>
    {
        options.Cookie.Name = "AdminInstallCookieAuth";
        options.LoginPath = "/Admin/Install";
    });

//Frontend Authentication
builder.Services.AddAuthentication()
    .AddCookie("FrontendCookie", options =>
    {
        options.Cookie.Name = "FrontendCookieAuth";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Events.OnRedirectToLogin = context =>
        {
            // Prevent redirect to login page
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            return Task.CompletedTask;
        };
    });

builder.Services.AddScoped<IInstallService, InstallService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<IAdminRoleService, AdminRoleService>();
builder.Services.AddScoped<IAdminRoleRepository, AdminRoleRepository>();

builder.Services.AddScoped<IEntityFilterService, EntityFilterService>();
builder.Services.AddScoped<IEntityHelperService, EntityHelperService>();
builder.Services.AddScoped<IEntitySearchService, EntitySearchService>();
builder.Services.AddScoped<IEntitySortService, EntitySortService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IAdminNavigationService, AdminNavigationService>();

builder.Services.AddScoped<IAdminRoleSeeder, AdminRoleSeeder>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IArticleTagMtmRepository, ArticleTagMtmRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddSingleton<IFrontendService, FrontendService>();
builder.Services.AddScoped<IUserFeedService, UserFeedService>();

builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("Site"));

// Frontend config
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["Site:FrontendUrl"])
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddHostedService<AggregateArticleViewService>();
builder.Services.AddHostedService<AggregateArticleRatingService>();
builder.Services.AddHostedService<AggregateCommentRatingService>();
builder.Services.AddHostedService<AggregateUserFollowService>();
builder.Services.AddHostedService<AggregateTagFollowService>();
builder.Services.AddHostedService<AggregateTagArticleService>();
builder.Services.AddHostedService<AggregateUserArticlesService>();
builder.Services.AddHostedService<AggregateCategoryArticlesService>();
builder.Services.AddHostedService<GenerateXmlSitemapService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseCors("AllowLocalhost");
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
    OnPrepareResponse = (context) =>
    {
        var headers = context.Context.Response.GetTypedHeaders();
        headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromDays(365)
        };
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GuestIdMiddleware>();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
