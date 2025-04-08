using System.Text;
using FluentValidation;
using FluentValidation.Results;
using FluxConfig.Management.Api.Extensions;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.User;

namespace FluxConfig.Management.Api.FiltersAttributes.Utils;

internal static class ExceptionLoggerExtensions
{
    internal static void LogApplicationException(this ILogger logger, Exception exception, DateTime curTime,
        string callId)
    {
        switch (exception)
        {
            // Auth / credentials
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