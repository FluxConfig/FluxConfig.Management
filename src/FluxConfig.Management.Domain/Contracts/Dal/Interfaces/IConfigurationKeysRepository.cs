using FluxConfig.Management.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IConfigurationKeysRepository: IDbRepository
{
    public Task AddKeys(ConfigurationKeyEntity[] entities, CancellationToken cancellationToken);
    public Task DeleteKey(string id, long configurationId, CancellationToken cancellationToken);
    public Task<IReadOnlyList<ConfigurationKeyEntity>> GetAllForConfiguration(long configurationId,DateTimeOffset currentUtcTime, CancellationToken cancellationToken);

    public Task<ConfigurationKeyEntity> GetValidConfigurationKey(string id, DateTimeOffset curTimeUtc,
        CancellationToken cancellationToken);
}