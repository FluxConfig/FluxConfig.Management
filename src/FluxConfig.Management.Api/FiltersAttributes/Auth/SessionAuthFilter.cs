using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Api.FiltersAttributes.Utils;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Models.Auth;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Models.User;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth;

public class SessionAuthFilter : IAsyncAuthorizationFilter
{
    private readonly IUserAuthService _authService;
    private readonly ILogger<SessionAuthFilter> _logger;
    private readonly UserGlobalRole _requiredRole;
    private readonly IRequestAuthContext _requestAuthContext;

    public SessionAuthFilter(
        IUserAuthService userAuthService,
        ILogger<SessionAuthFilter> logger,
        IRequestAuthContext authContext,
        UserGlobalRole requiredRole = UserGlobalRole.Member)
    {
        _authService = userAuthService;
        _logger = logger;
        _requestAuthContext = authContext;
        _requiredRole = requiredRole;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var cancellationToken = context.HttpContext.RequestAborted;

        try
        {
            UserModel user = await _authService.UserCheckAuth(
                sessionId: context.HttpContext.Request.Cookies[SessionModel.SessionCookieKey],
                cancellationToken: cancellationToken
            );

            if (user.Role < _requiredRole)
            {
                throw new UserUnauthenticatedException(
                    message: $"Dont have enough permissions to access resource. Required role: {_requiredRole.ToString()}",
                    reason: $"Dont have enough permissions to access resource. Required role: {_requiredRole.ToString()}"
                );
            }

            _requestAuthContext.User = user;
        }
        catch (UserUnauthenticatedException ex)
        {
            _logger.LogUserUnauthenticatedError(
                callId: context.HttpContext.TraceIdentifier,
                curTime: DateTime.Now,
                reason: ex.Reason
            );

            ErrorRequestHandler.HandleUserUnauthorizedRequest(
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