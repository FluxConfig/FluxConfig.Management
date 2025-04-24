using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ConfigAuthAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;
    public UserConfigRole RequiredRole { get; set; }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var configurationUsersService = serviceProvider.GetRequiredService<IConfigurationUsersService>();
        var logger = serviceProvider.GetRequiredService<ILogger<ConfigRoleAuthFilter>>();
        var context = serviceProvider.GetRequiredService<IRequestAuthContext>();

        return new ConfigRoleAuthFilter(
            configurationUsersService: configurationUsersService,
            logger: logger,
            requestAuthAuthContext: context,
            requiredRole: RequiredRole
        );
    }
}