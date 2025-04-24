using FluxConfig.Management.Api.Contracts.Requests.Configurations.Data;
using FluxConfig.Management.Api.Contracts.Requests.Configurations.Keys;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Data;
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

    internal static ConfigurationKeyValueType MapRequestToModel(this ConfigurationKeyValueRequest request)
    {
        return new ConfigurationKeyValueType
        {
            Key = request.Key,
            Value = request.Value,
            Type = request.Type
        };
    }
    
    internal static IReadOnlyList<ConfigurationKeyValueType> MapRequestsToModels(this IEnumerable<ConfigurationKeyValueRequest> requests)
    {
        return requests.Select(r => r.MapRequestToModel()).ToList();
    }
}