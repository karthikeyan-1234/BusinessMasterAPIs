
using PurchaseAPI.Services;

using PurchaseLibrary.Models;
using PurchaseLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PurchaseDbContext>();

builder.Services.AddScoped<IMaterialService,MaterialService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();

builder.Services.AddHostedService<PurchaseBackGroundService>();
builder.Services.AddStackExchangeRedisCache(opt => { opt.Configuration = "localhost:6379"; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
