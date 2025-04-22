using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationUsersService
{
    public Task<UserConfigurationRoleModel> CheckUserConfigPermissions(long userId, long configurationId, UserConfigRole requiredRole,
        CancellationToken cancellationToken);

    public Task AddUserToConfiguration(long configurationId, string userEmail, CancellationToken cancellationToken);
    public Task ChangeUserConfigurationRole(UserConfigurationRoleModel model, CancellationToken cancellationToken);
    public Task DeleteUserFromConfiguration(UserConfigurationRoleModel model, CancellationToken cancellationToken);
    public Task<IReadOnlyList<ConfigurationUsersViewModel>> GetConfigurationMembers(long configurationId,
        CancellationToken cancellationToken);
}