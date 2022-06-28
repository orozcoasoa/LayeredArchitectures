using AutoMapper;
using CatalogService.BLL.Setup;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
CatalogService.DAL.Configure.ConfigureServices(builder.Services, "Data Source=sample.db");
Configure.ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
