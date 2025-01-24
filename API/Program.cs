using API.GraphQL;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGraphQL();
builder.Services.AddScoped<IPakketRepo, PakketEFRepo>();
builder.Services.AddScoped<IKantineRepo, KantineEFRepo>();
builder.Services.AddScoped<IStudentRepo, StudentEFRepo>();
builder.Services.AddScoped<IMedewerkerRepo, MedewerkerEFRepo>();
builder.Services.AddScoped<IProductRepo, ProductEFRepo>();
builder.Services.AddScoped<IPakketService, PakketService>();
builder.Services.AddScoped<AuthFilter>();

// Configure database connection strings
var defaultConnection = string.Empty;
var identityConnection = string.Empty;

if (builder.Environment.IsDevelopment())
{
    // Use local connection string during development
    defaultConnection = builder.Configuration.GetConnectionString("LocalConnection");
    identityConnection = builder.Configuration.GetConnectionString("LocalIdentityConnection");

}
else
{
    // Use environment variable for production connection
    defaultConnection = builder.Configuration.GetConnectionString("LocalConnection");
    identityConnection = builder.Configuration.GetConnectionString("LocalIdentityConnection");
}

// Register DbContext with appropriate connection string
builder.Services.AddDbContext<FoodWasteDbContext>(options =>
    options.UseSqlServer(defaultConnection));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(identityConnection));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Configure GraphQL services
builder.Services.AddGraphQLServer()
    .AddQueryType<QueryType>()
    .AddType<PakketType>()
    .AddType<KantineType>()
    .AddType<StudentType>()
    .AddType<ProductType>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL("/graphql");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.Run();