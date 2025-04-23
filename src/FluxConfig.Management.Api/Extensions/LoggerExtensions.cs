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
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4023,
        Message = "[{CallId}] [{CurTime}] Configuration with id: {ConfigId} not found"
    )]
    public static partial void LogConfigurationNotFoundError(this ILogger logger,
        string callId,
        DateTime curTime,
        long configId);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4024,
        Message = "[{CallId}] [{CurTime}] Configuration key with id: {KeyId} not found"
    )]
    public static partial void LogConfigurationKeyNotFoundError(this ILogger logger,
        string callId,
        DateTime curTime,
        string keyId);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4025,
        Message = "[{CallId}] [{CurTime}] Configuration tag with id: {TagId} not found"
    )]
    public static partial void LogConfigurationTagNotFoundError(this ILogger logger,
        string callId,
        DateTime curTime,
        long tagId);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4026,
        Message = "[{CallId}] [{CurTime}] User with id: {UserId} doesnt belong to config with id: {ConfigId}"
    )]
    public static partial void LogUserDoesntBelongToConfigError(this ILogger logger,
        string callId,
        DateTime curTime,
        long userId,
        long configId);
    
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4027,
        Message = "[{CallId}] [{CurTime}] Admin with id: {AdminId} cant change his role."
    )]
    public static partial void LogAdminChangeHisRoleBadRequest(this ILogger logger,
        string callId,
        DateTime curTime,
        long adminId);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4028,
        Message = "[{CallId}] [{CurTime}] Configuration tag: {Tag} for configuration with id: {ConfigurationId} already exists"
    )]
    public static partial void LogConfigurationTagAlreadyExistsError(this ILogger logger,
        string callId,
        DateTime curTime,
        long configurationId,
        string tag);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = 4029,
        Message = "[{CallId}] [{CurTime}] Invalid configuration value: {Value} for key: {Key} with type: {Type}"
    )]
    public static partial void LogInvalidConfigurationValueForTypeError(this ILogger logger,
        string callId,
        DateTime curTime,
        string key,
        string value,
        string type);
    

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