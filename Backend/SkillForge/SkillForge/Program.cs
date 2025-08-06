using System.Text;
using System.Text.RegularExpressions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Areas.Admin.Services;
using SkillForge.Configuration;
using SkillForge.Cron;
using SkillForge.Data;
using SkillForge.Data.Seeders;
using SkillForge.Middleware;
using SkillForge.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .SetIsOriginAllowed(origin =>
                {
                    //Vercel-specific logic

                    if (origin == builder.Configuration["Site:FrontendUrl"])
                    {
                        return true;
                    }

                    Regex regex = new(@"skill-forge-[a-z0-9]{9,9}-roberts-projects-a59055d1\.vercel\.app");

                    if (regex.IsMatch(origin))
                    {
                        return true;
                    }

                    return false;
                })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Auth:Jwt:Issuer"],
            ValidAudience = builder.Configuration["Auth:Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:Jwt:Key"]))
        };
    });

builder.Services.AddScoped<IInstallService, InstallService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
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
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IArticleReportService, ArticleReportService>();
builder.Services.AddScoped<IArticleReportRepository, ArticleReportRepository>();
builder.Services.AddScoped<ICommentReportService, CommentReportService>();
builder.Services.AddScoped<ICommentReportRepository, CommentReportRepository>();
builder.Services.AddScoped<IUserReportService, UserReportService>();
builder.Services.AddScoped<IUserReportRepository, UserReportRepository>();
builder.Services.AddScoped<IGuestArticleViewService, GuestArticleViewService>();
builder.Services.AddScoped<IGuestArticleViewRepository, GuestArticleViewRepository>();
builder.Services.AddScoped<IArticleTagMtmRepository, ArticleTagMtmRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddSingleton<IFrontendService, FrontendService>();
builder.Services.AddScoped<IUserFeedService, UserFeedService>();

builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("Site"));

builder.Services.AddScoped<IAggregateArticleViewJob, AggregateArticleViewJob>();
builder.Services.AddScoped<IAggregateArticleRatingJob, AggregateArticleRatingJob>();
builder.Services.AddScoped<IAggregateCommentRatingJob, AggregateCommentRatingJob>();
builder.Services.AddScoped<IAggregateUserFollowJob, AggregateUserFollowJob>();
builder.Services.AddScoped<IAggregateTagFollowJob, AggregateTagFollowJob>();
builder.Services.AddScoped<IAggregateTagArticleJob, AggregateTagArticleJob>();
builder.Services.AddScoped<IAggregateUserArticlesJob, AggregateUserArticlesJob>();
builder.Services.AddScoped<IGenerateXmlSitemapJob, GenerateXmlSitemapJob>();

builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    app.UseExceptionHandler("/Dashboard/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GuestIdMiddleware>();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.UseHangfireDashboard("/Admin/Hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireAuthorizationFilter()
    }
});

CronConfiguration.Setup();

app.Run();
