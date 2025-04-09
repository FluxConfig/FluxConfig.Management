using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Api.FiltersAttributes.Utils;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using FluxConfig.Management.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ILogger<ApiKeyAuthFilter> _logger;

    public ApiKeyAuthFilter(IConfiguration configuration, IWebHostEnvironment environment,
        ILogger<ApiKeyAuthFilter> logger)
    {
        _configuration = configuration;
        _hostEnvironment = environment;
        _logger = logger;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            string expectedApiKey = ConfigurationGetter.GetInternalFcApiKey(
                configuration: _configuration,
                isDevelopment: _hostEnvironment.IsDevelopment());

            string? givenApiKey = context.HttpContext.Request.Headers["X-API-KEY"];

            if (givenApiKey == null || !string.Equals(expectedApiKey, givenApiKey, StringComparison.Ordinal))
            {
                throw new InternalApiKeyUnauthenticatedException(
                    message: "Invalid internal api-key metadata, needed to authenticate request to FC Management",
                    apiKey: givenApiKey ?? ""
                );
            }
        }
        catch (InternalApiKeyUnauthenticatedException ex)
        {
            _logger.LogInternalApiKeyAuthError(
                curTime: DateTime.Now,
                callId: context.HttpContext.TraceIdentifier,
                apiKey: ex.InvalidApiKey
            );

            ErrorRequestHandler.HandleInternalApiKeyUnauthenticated(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogInternalError(
                curTime: DateTime.Now,
                callId: context.HttpContext.TraceIdentifier,
                exception: ex
            );
            
            ErrorRequestHandler.HandleInternalError(context);
        }
    }
}