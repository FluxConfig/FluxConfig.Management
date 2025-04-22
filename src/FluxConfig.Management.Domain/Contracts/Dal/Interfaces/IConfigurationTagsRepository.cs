using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IConfigurationTagsRepository: IDbRepository
{
    public Task<ConfigurationTagsViewEntity> GetTagWithConfigurationByConfigId(long configurationId, string tag,
        CancellationToken cancellationToken);
}