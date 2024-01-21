using Core.DomainServices.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using API.GraphQL;
using Core.DomainServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGraphQL();
builder.Services.AddScoped<IPakketRepo, PakketEFRepo>();
builder.Services.AddScoped<IStudentRepo, StudentEFRepo>();
builder.Services.AddScoped<IKantineRepo, KantineEFRepo>();
builder.Services.AddScoped<IProductRepo, ProductEFRepo>();
builder.Services.AddScoped<IPakketService, PakketService>();


builder.Services.AddDbContext<FoodWasteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddGraphQLServer()
    .AddQueryType<QueryType>()
    .AddType<PakketType>()
    .AddType<StudentType>()
    .AddType<KantineType>()
    .AddType<ProductType>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/graphql");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
