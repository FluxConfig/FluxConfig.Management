using System.Text.Json;
using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.Middleware;
using FluxConfig.Management.Domain.DependencyInjection.Extensions;

namespace FluxConfig.Management.Api;

public sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _hostEnvironment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDomainServices()
            .AddGlobalFilters()
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            })
            .AddMvcOptions(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ApiKeyAuthFilter>();
            });
    }
    
    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<TracingMiddleware>();
        app.UsePathBase("/api/fcm");
        app.UseRouting();

        app.UseMiddleware<LoggingMiddleware>();
        
        //TODO: Добавить корс для веб клиента
        app.UseCors();

        app.UseEndpoints(endpointBuilder =>
        {
            endpointBuilder.MapControllers();
        });
    }

}