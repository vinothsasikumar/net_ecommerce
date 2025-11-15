using Ecommerce.Api.Middlwares;
using Ecommerce.Api.Services;
using Ecommerce.Api.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration.AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true).Build();

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    builder.Host.UseSerilog(logger);

    builder.Services.AddControllers();

    builder.Services.AddScoped<IProductService, ProductService>();

    builder.Services.AddSwaggerGen(config =>
    {
        config.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Ecommerce API",
            Contact = new OpenApiContact
            {
                Name = "Meena",
                Email = "meena@ingram.com"
            },
            Description = "This API exposes endpoints for eCommerce project",
        });
    });

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API");
    });

    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseMiddleware<ErrorLogger>();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal($"Unhandled Exception: {ex.Message}");
}
finally
{
    Log.Information("Shutting down the application");
    Log.CloseAndFlush();
}