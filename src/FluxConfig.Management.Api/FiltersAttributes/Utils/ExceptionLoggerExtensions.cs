using System.Text;
using FluentValidation;
using FluentValidation.Results;
using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

namespace FluxConfig.Management.Api.FiltersAttributes.Utils;

internal static class ExceptionLoggerExtensions
{
    internal static void LogApplicationException(this ILogger logger, Exception exception, DateTime curTime,
        string callId)
    {
        switch (exception)
        {
            // Configuration
            case ConfigurationTagAlreadyExistsException ex:
                logger.LogConfigurationTagAlreadyExistsError(
                    callId: callId,
                    curTime: curTime,
                    configurationId: ex.ConfigurationId,
                    tag: ex.Tag
                );
                break;

            case UserConfigUnauthorizedException ex:
                logger.LogUserUnauthenticatedError(
                    callId: callId,
                    curTime: curTime,
                    reason: ex.Reason
                );

                break;

            case UserDoesntBelongToConfigException ex:
                logger.LogUserDoesntBelongToConfigError(
                    callId: callId,
                    curTime: curTime,
                    userId: ex.UserId,
                    configId: ex.ConfigId
                );
                break;

            case ConfigurationNotFoundException ex:
                logger.LogConfigurationNotFoundError(
                    callId: callId,
                    curTime: curTime,
                    configId: ex.ConfigurationId
                );
                break;

            case ConfigurationKeyNotFoundException ex:

                logger.LogConfigurationKeyNotFoundError(
                    callId: callId,
                    curTime: curTime,
                    keyId: ex.KeyId
                );
                break;

            case ConfigurationTagNotFoundException ex:

                logger.LogConfigurationTagNotFoundError(
                    callId: callId,
                    curTime: curTime,
                    tagId: ex.TagId
                );
                break;

            // ISC - Fc Storage
            case FcStorageResponseException ex:

                logger.LogFcStorageUnexpectedResponseError(
                    callId: callId,
                    curTime: curTime,
                    statusCode: ex.StatusCode
                );

                break;

            case FcStorageInternalApiKeyUnauthenticatedException ex:

                logger.LogInternalServiceOutgoingAuthError(
                    callId: callId,
                    curTime: curTime,
                    apiKey: ex.InvalidApiKey
                );

                break;

            // Auth / credentials
            case AdminChangeHisRoleException ex:

                logger.LogAdminChangeHisRoleBadRequest(
                    callId: callId,
                    curTime: curTime,
                    adminId: ex.AdminId
                );

                break;

            case InvalidUserCredentialsException ex:

                logger.LogInvalidUserCredentialsError(
                    callId: callId,
                    curTime: curTime,
                    email: ex.Email,
                    invalidCredentials: ex.InvalidPassword
                );

                break;

            case UserAlreadyExistsException ex:

                logger.LogUserAlreadyExistsError(
                    curTime: curTime,
                    callId: callId,
                    email: ex.Email
                );

                break;

            case UserNotFoundException ex:

                logger.LogUserNotFoundError(
                    curTime: curTime,
                    callId: callId,
                    invalidEmail: ex.InvalidEmail
                );

                break;

            case UserUnauthenticatedException ex:

                logger.LogUserUnauthenticatedError(
                    curTime: curTime,
                    callId: callId,
                    reason: ex.Reason
                );

                break;

            // General 
            case BadRequestException ex:

                logger.LogDomainBadRequestError(
                    curTime: curTime,
                    callId: callId,
                    violations: QueryUnvalidatedFields(ex.ValidationException)
                );

                break;

            default:

                logger.LogInternalError(
                    exception: exception,
                    curTime: curTime,
                    callId: callId
                );

                break;
        }
    }

    private static string QueryUnvalidatedFields(
        ValidationException? exception)
    {
        StringBuilder validationExceptionsBuilder = new StringBuilder();

        if (exception == null)
        {
            return string.Empty;
        }

        foreach (ValidationFailure failure in exception.Errors)
        {
            validationExceptionsBuilder.Append($">> Field: {failure.PropertyName}, Error: {failure.ErrorMessage}\n");
        }

        return validationExceptionsBuilder.ToString();
    }
}