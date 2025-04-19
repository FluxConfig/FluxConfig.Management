using System.Net;
using FluentValidation;
using FluentValidation.Results;
using FluxConfig.Management.Api.Contracts.Responses;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluxConfig.Management.Api.FiltersAttributes.Utils;

internal static class ErrorRequestHandler
{
    #region User
    
    internal static void HandleUserNotFoundRequest(ExceptionContext context, UserNotFoundException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.NotFound,
                Message: $"User with{(ex.InvalidEmail == null ? "" : $" email: {ex.InvalidEmail}")} {(ex.Id == -1 ? "" :  $" ID: {ex.Id}")} not found",
                Exceptions: [ex.InvalidEmail ?? "", (ex.Id == -1 ? "" : ex.Id.ToString())]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.NotFound
        };

        context.Result = result;
    }
    
    internal  static void HandleUserConflictRequest(ExceptionContext context, UserAlreadyExistsException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.Conflict,
                Message: $"User with email: {ex.Email} already exists",
                Exceptions: [ex.Email]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.Conflict
        };

        context.Result = result;
    }

    internal static void HandleUserInvalidCredentials(ExceptionContext context, InvalidUserCredentialsException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.BadRequest,
                Message: "Invalid credentials provided.",
                Exceptions: [ex.InvalidPassword ?? ""]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.BadRequest
        };

        context.Result = result;
    }
    
    #endregion

    #region User-Auth
    
    internal static void HandleUserUnauthorizedRequest(AuthorizationFilterContext context, UserUnauthenticatedException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.Unauthorized,
                Message: $"Unauthorized to access resource. {ex.Reason}",
                Exceptions: [ex.Reason]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        context.Result = result;
    }
    
    internal static void HandleUserUnauthorizedRequest(ExceptionContext context, UserUnauthenticatedException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.Unauthorized,
                Message: $"Unauthorized to access resource. {ex.Reason}",
                Exceptions: [ex.Reason]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        context.Result = result;
    }

    #endregion

    #region Internal auth

    internal static void HandleInternalApiKeyUnauthenticated(AuthorizationFilterContext context,
        InternalApiKeyUnauthenticatedException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.Unauthorized,
                Message: $"Invalid internal authentication metadata provided. X-API-KEY: {ex.InvalidApiKey}",
                Exceptions: [ex.InvalidApiKey]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        context.Result = result;
    }

    #endregion
    
    #region General
    
    internal static void HandleBadRequest(ExceptionContext context, BadRequestException ex)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.BadRequest,
                Message: "Invalid request parameters.",
                Exceptions: QueryUnvalidatedFields(ex.ValidationException)
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.BadRequest
        };

        context.Result = result;
    }

    internal static void HandleInternalError(ExceptionContext context)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.InternalServerError,
                Message: "Internal error. Check FC Management logs for detailed description.",
                Exceptions: ["Check FC Management logs for detailed description."]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        context.Result = result;
    }

    internal static void HandleInternalError(AuthorizationFilterContext context)
    {
        JsonResult result = new JsonResult(
            new ErrorResponse(
                StatusCode: HttpStatusCode.InternalServerError,
                Message: "Internal error. Check FC Management logs for detailed description.",
                Exceptions: ["Check FC Management logs for detailed description."]
            )
        )
        {
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        context.Result = result;
    }

    #endregion

    private static IEnumerable<string> QueryUnvalidatedFields(
        ValidationException exception)
    {
        foreach (ValidationFailure failure in exception.Errors)
        {
            yield return $">> Field: {failure.PropertyName}, Error: {failure.ErrorMessage}\n";
        }
    }
}