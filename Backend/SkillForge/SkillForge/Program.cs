using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Data.Seeders;
using SkillForge.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
