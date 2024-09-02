using Microsoft.AspNetCore.Cors.Infrastructure;

using VendorLibrary.Models;
using VendorLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IVendorChangeNotification, VendorChangeNotification>();
builder.Services.AddDbContext<VendorDbContext>();

CorsPolicyBuilder cbuilder = new CorsPolicyBuilder().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200").AllowCredentials();
CorsPolicy policy = cbuilder.Build();

builder.Services.AddCors(opt => {
    opt.AddPolicy("MyCors", policy);
});

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

app.UseCors("MyCors");

app.Run();
