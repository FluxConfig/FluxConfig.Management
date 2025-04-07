using System.Net;
using System.Text.Json;
using FluxConfig.Management.Api.FiltersAttributes;
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
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            })
            .AddMvcOptions(options =>
            {
                //TODO: Добавить ExceptionFilter с версионированным логированием и маппингом в еррор респонс
                // в логировании наебенить трейсинг запросов
                options.Filters.Add(new ErrorResponseTypeAttribute((int)HttpStatusCode.NotFound));
                options.Filters.Add(new ErrorResponseTypeAttribute((int)HttpStatusCode.BadRequest));
            });
    }
    
    public void Configure(IApplicationBuilder app)
    {
        app.UsePathBase("/api/fcm");
        app.UseRouting();

        //TODO: Добавить мидлвар логирования с трейсингом
        //TODO: Добавить корс для веб клиента
        app.UseCors();

        app.UseEndpoints(endpointBuilder =>
        {
            endpointBuilder.MapControllers();
        });
    }

}