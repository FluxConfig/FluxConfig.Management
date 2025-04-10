using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;
    public UserGlobalRole RequiredRole { get; set; }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var userCredentialsService = serviceProvider.GetRequiredService<IUserCredentialsService>();
        var logger = serviceProvider.GetRequiredService<ILogger<SessionAuthFilter>>();
        var context = serviceProvider.GetRequiredService<IRequestContext>();
        return new SessionAuthFilter(userCredentialsService, logger, context, RequiredRole);
    }
}