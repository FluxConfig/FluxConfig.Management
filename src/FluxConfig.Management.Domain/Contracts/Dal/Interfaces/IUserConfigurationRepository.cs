using FluxConfig.Management.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IUserConfigurationRepository: IDbRepository
{
    public Task<UserConfigurationEntity> GetUserConfiguration(long userId, long configurationId,
        CancellationToken cancellationToken);
}