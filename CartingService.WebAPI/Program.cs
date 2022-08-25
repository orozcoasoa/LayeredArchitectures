using System.Reflection;
using CartingService.BLL.Setup;
using CartingService.DAL;
using CartingService.WebAPI;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new SwaggerGroupByVersion());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "CartingService API",
        Version = "v1",
        Description = "Web API service for cart management."
    });
    options.SwaggerDoc("v2", new OpenApiInfo()
    {
        Title = "CartingService API",
        Version = "v2",
        Description = "Web API service for cart management."
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.ConfigureDAL()
                .ConfigureBLL();

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
