using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineSalesAuth.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlineSaleSiteAuth.Application;
using OnlineSaleSiteAuth.Application.Service.Claim;
using OnlineSaleSiteAuth.Application.Service.Product;
using OnlineSaleSiteAuth.Domain;
using OnlineSaleSiteAuth.Persistence;
using OnlineSaleSiteAuth.Application.Service.Category;
using Microsoft.AspNetCore.Hosting;
using OnlineSaleSiteAuth.Application.Service.File;
using OnlineSaleSiteAuth.Application.Service.Basket;
using OnlineSaleSiteAuth.Application.Service.Coupon;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(
    option => option
                    .UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"))
                    .AddInterceptors(new SoftDeleteInterceptor()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IClaimManager, ClaimManager>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<ICouponService, CouponService>();


builder.Services.AddSession();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Product/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
