using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationUsersService : IConfigurationUsersService
{
    private readonly IUserConfigurationRepository _userConfigurationRepository;

    public ConfigurationUsersService(IUserConfigurationRepository userConfigurationRepository)
    {
        _userConfigurationRepository = userConfigurationRepository;
    }

    public async Task<UserConfigurationRoleModel> CheckUserConfigPermissions(long userId, long configurationId,
        UserConfigRole requiredRole,
        CancellationToken cancellationToken)
    {
        try
        {
            return await CheckUserConfigPermissionsUnsafe(userId, configurationId, requiredRole, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserConfigUnauthorizedException(
                message: "Dont have enough permissions to access resource.",
                reason: "User doesnt belong to configuration.",
                innerException: ex
            );
        }
    }

    private async Task<UserConfigurationRoleModel> CheckUserConfigPermissionsUnsafe(long userId, long configurationId,
        UserConfigRole requiredRole,
        CancellationToken cancellationToken)
    {
        using var transaction = _userConfigurationRepository.CreateTransactionScope();

        UserConfigurationEntity entity = await _userConfigurationRepository.GetUserConfiguration(
            userId: userId,
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        if (entity.Role < requiredRole)
        {
            throw new UserConfigUnauthorizedException(
                message: "Dont have enough permissions to access resource.",
                reason: $"Required role: {requiredRole.ToString()}"
            );
        }

        return new UserConfigurationRoleModel(
            UserId: entity.UserId,
            Role: entity.Role,
            ConfigurationId: entity.ConfigurationId
        );
    }
}