using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddDataProtection(); // Şifreleme yapılacak.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/"); // oturum açınca ana sayfaya at
    options.LogoutPath = new PathString("/"); // oturum kapatılınca ana sayfaya at
    options.AccessDeniedPath = new PathString("/"); // yetkim olmayan bir sayfaya gitmek istiyorsam , ana sayfaya at.
});

var app = builder.Build();


app.UseAuthentication(); // Kimlik Oluşturma
app.UseAuthorization(); // Yetkilendirme

app.UseStaticFiles(); // wwwroot kullanımı için

app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");
// LocalHost:1231/Errors/Error404

app.MapControllerRoute(
    name: "areas",
    pattern: ("{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    );

app.MapControllerRoute(
    name: "default",
    pattern: ("{controller=home}/{action=index}/{id?}")
    );



app.Run();



// https://www.Bilgeshop.com/anasayfa
// https://www.Bilgeshop.com/admin/anasayfa -> area

