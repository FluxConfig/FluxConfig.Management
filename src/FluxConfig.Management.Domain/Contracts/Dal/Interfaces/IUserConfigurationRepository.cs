using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IUserConfigurationRepository: IDbRepository
{
    public Task<UserConfigurationEntity> GetUserConfiguration(long userId, long configurationId,
        CancellationToken cancellationToken);
    public Task AddUsersToConfiguration(UserConfigurationEntity[] entities, CancellationToken cancellationToken);
    public Task UpdateUserConfigRole(UserConfigurationEntity entity, CancellationToken cancellationToken);
    public Task DeleteUserFromConfiguration(long userId, long configurationId, CancellationToken cancellationToken);

    public Task<IReadOnlyList<ConfigurationUserViewEntity>> GetConfigurationUsers(long configurationId,
        CancellationToken cancellationToken);
}