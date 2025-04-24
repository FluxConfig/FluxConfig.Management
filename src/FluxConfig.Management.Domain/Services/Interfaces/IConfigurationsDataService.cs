using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationsDataService
{
    public Task UpdateConfigurationData(long tagId, UserConfigRole userRole, ConfigurationDataType dataType, IReadOnlyList<ConfigurationKeyValueType> updatedData,
        CancellationToken cancellationToken);

    public Task<IReadOnlyList<ConfigurationKeyValueType>> LoadConfigurationData(long tagId, UserConfigRole userRole,
        ConfigurationDataType dataType, CancellationToken cancellationToken);
}