using FluxConfig.Management.Api.FiltersAttributes.Utils;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        string callId = context.HttpContext.TraceIdentifier;

        _logger.LogApplicationException(
            exception: context.Exception,
            curTime: DateTime.Now,
            callId: callId
        );

        switch (context.Exception)
        {
            // Configuration
            case ClientServiceApiKeyUnauthorizedException ex:
                ErrorRequestHandler.HandleClientServiceApiKeyUnauthorizedBr(context, ex);
                break;
            
            case ConfigurationDataInvalidTypeException ex:
                ErrorRequestHandler.HandleInvalidConfigurationTypeValueBr(context, ex);
                break;
            
            case ConfigurationTagAlreadyExistsException ex:
                ErrorRequestHandler.HandleConfigurationTagAlEConflict(context, ex);
                break;
            
            case UserConfigUnauthorizedException ex:
                ErrorRequestHandler.HandleUserConfigActionUnauthorizedRequest(context, ex);
                break;
            
            case UserDoesntBelongToConfigException ex:
                ErrorRequestHandler.HandleUserDoesntBelongToConfigNotFoundRequest(context, ex);
                break;
            
            case ConfigurationNotFoundException ex:
                ErrorRequestHandler.HandleConfigurationNotFoundRequest(context, ex);
                break;
            
            case ConfigurationKeyNotFoundException ex:
                ErrorRequestHandler.HandleConfigurationKeyNotFoundRequest(context, ex);
                break;
            
            case ConfigurationTagNotFoundException ex:
                ErrorRequestHandler.HandleConfigurationTagNotFoundRequest(context, ex);
                break;
            
            // ISC - FcStorage
            case FcStorageInternalApiKeyUnauthenticatedException:
                ErrorRequestHandler.HandleInternalError(context);
                break;
            
            case FcStorageResponseException:
                ErrorRequestHandler.HandleInternalError(context);
                break;
            
            // User
            case AdminChangeHisRoleException ex:
                ErrorRequestHandler.HandleAdminChangeHisRoleBrRequest(context, ex);
                break;
            
            case UserAlreadyExistsException ex:
                ErrorRequestHandler.HandleUserConflictRequest(context, ex);
                break;
            
            case UserNotFoundException ex:
                ErrorRequestHandler.HandleUserNotFoundRequest(context, ex);
                break;
            
            case InvalidUserCredentialsException ex:
                ErrorRequestHandler.HandleUserInvalidCredentials(context, ex);
                break;
            
            // User-Auth
            case UserUnauthenticatedException ex:
                ErrorRequestHandler.HandleUserUnauthorizedRequest(context, ex);
                break;
            
            //General
            case BadRequestException ex:
                ErrorRequestHandler.HandleBadRequest(context, ex);
                break;
                
            default:
                ErrorRequestHandler.HandleInternalError(context);
                break;
        }
    }
}