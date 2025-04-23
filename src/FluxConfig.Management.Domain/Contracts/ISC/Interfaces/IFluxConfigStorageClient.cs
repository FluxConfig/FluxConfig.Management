using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.ISC.Interfaces;

public interface IFluxConfigStorageClient
{
    public Task CreateConfiguration(string key, string tag, CancellationToken cancellationToken);

    public Task DeleteConfiguration(string key, List<string> tags, CancellationToken cancellationToken);

    public Task<IReadOnlyList<ConfigurationKeyValueType>> LoadConfigData(string key, string tag,
        ConfigurationDataType dataType, CancellationToken cancellationToken);

    public Task UpdateConfigData(string key, string tag, IReadOnlyList<ConfigurationKeyValueType> updatedData,
        ConfigurationDataType dataType, CancellationToken cancellationToken);
}