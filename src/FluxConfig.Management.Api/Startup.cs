using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.Middleware;
using FluxConfig.Management.Domain.DependencyInjection.Extensions;
using FluxConfig.Management.Infrastructure.DependencyInjection.Extensions;

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
            .AddGlobalFilters()
            .AddCustomCors(
                configuration: _configuration,
                isDevelopment: _hostEnvironment.IsDevelopment())
            .AddUserAuthContext()
            .AddStorageClient(
                configuration: _configuration,
                isDevelopment: _hostEnvironment.IsDevelopment()
            )
            .AddDalInfrastructure(
                configuration: _configuration,
                isDevelopment: _hostEnvironment.IsDevelopment()
            )
            .AddDalRepositories()
            .AddDomainServices()
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddMvcOptions(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ApiKeyAuthFilter>();
            });
    }

    public void Configure(IApplicationBuilder app)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        app.UseMiddleware<TracingMiddleware>();
        app.UsePathBase("/api/fcm");
        app.UseRouting();

        app.UseMiddleware<LoggingMiddleware>();

        app.UseCors();

        app.UseEndpoints(endpointBuilder => { endpointBuilder.MapControllers(); });
    }
}