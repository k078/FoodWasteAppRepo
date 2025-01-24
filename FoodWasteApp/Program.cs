using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("nl-NL");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPakketRepo, PakketEFRepo>();
builder.Services.AddScoped<IPakketService, PakketService>();
builder.Services.AddScoped<IProductRepo, ProductEFRepo>();
builder.Services.AddScoped<IKantineRepo, KantineEFRepo>();
builder.Services.AddScoped<IStudentRepo, StudentEFRepo>();
builder.Services.AddScoped<IMedewerkerRepo, MedewerkerEFRepo>();

var defaultconnection = string.Empty;
var identityconnection = string.Empty;

if (builder.Environment.IsDevelopment())
{
    defaultconnection = builder.Configuration.GetConnectionString("LocalConnection");
    identityconnection = builder.Configuration.GetConnectionString("LocalIdentityConnection");
}
else
{
    defaultconnection = builder.Configuration.GetConnectionString("LocalDefaultConnection");
    identityconnection= builder.Configuration.GetConnectionString("LocalIdentityConnection");
}

builder.Services.AddDbContext<FoodWasteDbContext>(options => options.UseSqlServer(defaultconnection));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(identityconnection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options=> {
    options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<IdentityContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
