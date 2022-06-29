using CartingService.WebAPI;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "CartingService API",
        Version = "v1",
        Description = "Web API service for cart management."
    });
    c.SwaggerDoc("v2", new OpenApiInfo()
    {
        Title = "CartingService API",
        Version = "v2",
        Description = "Web API service for cart management."
    });
});
CartingService.DAL.Configure.ConfigureServices(builder.Services, "Sample.db");
CartingService.BLL.Configure.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CartingService.v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "CartingService.v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
