using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IConfigurationsRepository: IDbRepository
{
    public Task<IReadOnlyList<UserConfigurationsViewEntity>> GetUserConfigurations(long userId, CancellationToken cancellationToken);
    public Task<IReadOnlyList<ConfigurationEntity>> GetAllConfigurations(CancellationToken cancellationToken);
    public Task ChangeConfigurationName(long id, string newName, CancellationToken cancellationToken);
    public Task ChangeConfigurationDescription(long id, string newDescription, CancellationToken cancellationToken);

    public Task<UserConfigurationsViewEntity> GetUserConfigurationById(long userId, long configurationId,
        CancellationToken cancellationToken);
}