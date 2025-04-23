using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IConfigurationTagsRepository: IDbRepository
{
    public Task<ConfigurationTagsViewEntity> GetTagWithConfigurationByConfigId(long configurationId, string tag,
        CancellationToken cancellationToken);
    public Task<ConfigurationTagsViewEntity> GetTagWithConfigurationByTagId(long tagId,
        CancellationToken cancellationToken);
    public Task<IReadOnlyList<ConfigurationTagEntity>> GetConfigurationTags(long configurationId,
        CancellationToken cancellationToken);
    public Task UpdateTagDescription(long tagId, string newDescription, CancellationToken cancellationToken);
    public Task UpdateTagRequiredRole(long tagId, UserConfigRole newRole, CancellationToken cancellationToken);
}