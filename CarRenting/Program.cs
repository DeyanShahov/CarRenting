using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Infrastructure;
using CarRenting.Services.Cars;
using CarRenting.Services.Dealers;
using CarRenting.Services.Statistics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CarRentingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CarRentingDbContext>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services
    .AddTransient<IStatisticsService, StatisticsService>()
    .AddTransient<ICarSevice, CarService>()
    .AddTransient<IDealerService, DealerService>();

//Add forgery security
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

// Add AtoMapper to work
builder.Services.AddAutoMapper(typeof(Program));

//Add InMemory cashing
builder.Services.AddMemoryCache();

//Add sesion
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(10);
    //XSS security
    option.Cookie.HttpOnly = true;
});



var app = builder.Build();

// Auto migration on DB if DbContext have changes
app.PrepareDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.MapRazorPages();

app.Run();
