using FluxConfig.Management.Api.Contracts.Requests.Configurations.Keys;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Api.Mappers.Requests;

internal static class ConfigRequestMapper
{
    internal static ConfigurationKeyModel MapRequestToModel(this CreateConfigurationApiKeyRequest request,
        long configurationId)
    {
        return new ConfigurationKeyModel(
            Id: "",
            ConfigurationId: configurationId,
            RolePermission: request.RolePermission,
            ExpirationDate: ConvertToUtcWithZeroOffset(request.ExpirationDate)
        );
    }
    
    private static DateTimeOffset ConvertToUtcWithZeroOffset(DateTime dateTime)
    {
        DateTime utcDateTime = dateTime.Kind == DateTimeKind.Utc 
            ? dateTime 
            : dateTime.ToUniversalTime();

        return new DateTimeOffset(utcDateTime, TimeSpan.Zero);
    }
}