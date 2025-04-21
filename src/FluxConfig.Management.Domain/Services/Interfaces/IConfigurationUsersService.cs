using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationUsersService
{
    public Task<UserConfigurationRoleModel> CheckUserConfigPermissions(long userId, long configurationId, UserConfigRole requiredRole,
        CancellationToken cancellationToken);
}