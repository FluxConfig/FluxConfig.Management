namespace FluxConfig.Management.Api.Extensions;

internal static partial class LoggerExtensions
{
    # region Info

    [LoggerMessage(
        LogLevel.Information,
        EventId = 2000,
        Message = "[{CallId}] [{CurTime}] Start executing call. Endpoint: {EndpointRoute}"
        )]
    public static partial void LogRequestStart(this ILogger logger,
        DateTime curTime,
        string callId,
        string endpointRoute);


    [LoggerMessage(
        LogLevel.Information,
        EventId = 2001,
        Message = "[{CallId}] [{CurTime}] Ended executing call. Endpoint: {EndpointRoute}"
    )]
    public static partial void LogRequestEnd(this ILogger logger,
        DateTime curTime,
        string callId,
        string endpointRoute);

    # endregion

    #region Error

    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4000,
        Message = "[{CallId}] [{CurTime}] Request validation error: \n{Violations}"
    )]
    public static partial void LogDomainBadRequestError(this ILogger logger,
        string callId,
        DateTime curTime,
        string violations);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4010,
        Message = "[{CallId}] [{CurTime}] User with email: {InvalidEmail} not found"
    )]
    public static partial void LogUserNotFoundError(this ILogger logger,
        string callId,
        DateTime curTime,
        string? invalidEmail);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4011,
        Message = "[{CallId}] [{CurTime}] User with email: {Email} already exists"
    )]
    public static partial void LogUserAlreadyExistsError(this ILogger logger,
        string callId,
        DateTime curTime,
        string? email);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4012,
        Message = "[{CallId}] [{CurTime}] Invalid login credentials - {InvalidCredentials} provided for user with email: {Email}"
    )]
    public static partial void LogInvalidUserCredentialsError(this ILogger logger,
        string callId,
        DateTime curTime,
        string? email,
        string? invalidCredentials);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4013,
        Message = "[{CallId}] [{CurTime}] User unauthorized to access resource. Reason: {Reason}"
    )]
    public static partial void LogUserUnauthenticatedError(this ILogger logger,
        string callId,
        DateTime curTime,
        string reason);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4001,
        Message = "[{CallId}] [{CurTime}] Unexpected exception occured during request processing."
    )]
    public static partial void LogInternalError(this ILogger logger,
        Exception exception,
        string callId,
        DateTime curTime);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4020,
        Message =
            "[{CallId}] [{CurTime}] Invalid internal api-key authentication metadata needed to access FC Storage api. X-API-KEY: {ApiKey}"
    )]
    public static partial void LogInternalServiceOutgoingAuthError(this ILogger logger,
        string callId,
        DateTime curTime,
        string apiKey);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4021,
        Message =
            "[{CallId}] [{CurTime}] Given unexpected response from FC Storage service with status-code: {StatusCode}"
    )]
    public static partial void LogFcStorageUnexpectedResponseError(this ILogger logger,
        string callId,
        DateTime curTime,
        int statusCode);
    
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4022,
        Message =
            "[{CallId}] [{CurTime}] Given invalid internal api-key authentication metadata needed to access FC Management api. X-API-KEY: {ApiKey}"
    )]
    public static partial void LogInternalApiKeyAuthError(this ILogger logger,
        string callId,
        DateTime curTime,
        string apiKey);

    #endregion
    
    #region Debug

    [LoggerMessage(
        LogLevel.Debug,
        EventId = 1000,
        Message = "[{CallId}] [{CurTime}] Request headers:\n{Headers}"
    )]
    public static partial void LogRequestHeaders(this ILogger logger,
        DateTime curTime,
        string callId,
        string headers);

    #endregion
}