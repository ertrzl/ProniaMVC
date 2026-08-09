using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();



app.UseStaticFiles();
app.MapControllerRoute(
    "default",
    "{area:exists}/{controller=home}/{action=index}/{id?}"
);

app.MapControllerRoute(
    "default",
    "{controller=home}/{action=index}/{id?}"


    );

app.Run();
