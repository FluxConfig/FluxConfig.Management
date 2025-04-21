using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Api.FiltersAttributes.Utils;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth;

public class ConfigRoleAuthFilter : IAsyncAuthorizationFilter
{
    private const string ConfigurationIdHeader = "X-CONF-ID";
    private readonly IConfigurationUsersService _configurationUsersService;
    private readonly ILogger<ConfigRoleAuthFilter> _logger;
    private readonly IRequestAuthContext _requestAuthAuthContext;
    private readonly UserConfigRole _requiredRole;

    public ConfigRoleAuthFilter(
        IConfigurationUsersService configurationUsersService,
        ILogger<ConfigRoleAuthFilter> logger,
        IRequestAuthContext requestAuthAuthContext,
        UserConfigRole requiredRole = UserConfigRole.Member
    )
    {
        _configurationUsersService = configurationUsersService;
        _logger = logger;
        _requestAuthAuthContext = requestAuthAuthContext;
        _requiredRole = requiredRole;
    }


    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var cancellationToken = context.HttpContext.RequestAborted;

        try
        {
            string? configurationIdString = context.HttpContext.Request.Headers[ConfigurationIdHeader];

            if (configurationIdString == null || !long.TryParse(configurationIdString, out var configurationId))
            {
                throw new UserConfigUnauthorizedException(
                    message: "Dont have enough permissions to access resource.",
                    reason: $"Invalid passed {ConfigurationIdHeader} header."
                );
            }

            if (_requestAuthAuthContext.User!.Role != UserGlobalRole.Admin)
            {
                UserConfigurationRoleModel model = await _configurationUsersService.CheckUserConfigPermissions(
                    userId: _requestAuthAuthContext.User!.Id,
                    configurationId: configurationId,
                    requiredRole: _requiredRole,
                    cancellationToken: cancellationToken
                );

                _requestAuthAuthContext.ConfigurationRole = model;
            }
            else
            {
                _requestAuthAuthContext.ConfigurationRole = new UserConfigurationRoleModel(
                    UserId: _requestAuthAuthContext.User!.Id,
                    Role: UserConfigRole.Admin,
                    ConfigurationId: configurationId
                );
            }
        }
        catch (UserConfigUnauthorizedException ex)
        {
            _logger.LogUserUnauthenticatedError(
                callId: context.HttpContext.TraceIdentifier,
                curTime: DateTime.Now,
                reason: ex.Reason
            );

            ErrorRequestHandler.HandleUserConfigActionUnauthorizedRequest(
                context: context,
                ex: ex
            );
        }
        catch (Exception ex)
        {
            _logger.LogInternalError(
                callId: context.HttpContext.TraceIdentifier,
                curTime: DateTime.Now,
                exception: ex
            );

            ErrorRequestHandler.HandleInternalError(context);
        }
    }
}