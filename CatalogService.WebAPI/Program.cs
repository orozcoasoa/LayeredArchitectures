using CatalogService.BLL.Setup;
using CatalogService.DAL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Identity.Web;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDAL()
    .ConfigureBLL();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi(opt => opt.EnablePiiLogging = false)
                .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
                .AddInMemoryTokenCaches();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(excHApp =>
{
    excHApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = Text.Plain;
        var exception = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exception?.Error is KeyNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(exception.Error.Message);
        }
        else if (exception?.Error is ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(exception.Error.Message);
        }
        else
        {
            await context.Response.WriteAsync("Exception was thrown");
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
