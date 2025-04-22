using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationUsersService : IConfigurationUsersService
{
    private readonly IUserConfigurationRepository _userConfigurationRepository;
    private readonly IUserRepository _userRepository;

    public ConfigurationUsersService(IUserConfigurationRepository userConfigurationRepository,
        IUserRepository userRepository)
    {
        _userConfigurationRepository = userConfigurationRepository;
        _userRepository = userRepository;
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

    public async Task AddUserToConfiguration(long configurationId, string userEmail,
        CancellationToken cancellationToken)
    {
        try
        {
            await AddUserToConfigurationUnsafe(configurationId, userEmail, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {configurationId} could not be found.",
                configurationId: configurationId,
                innerException: ex
            );
        }
    }

    private async Task AddUserToConfigurationUnsafe(long configurationId, string userEmail,
        CancellationToken cancellationToken)
    {
        using var transaction = _userConfigurationRepository.CreateTransactionScope();

        UserCredentialsEntity userEntity;
        try
        {
            userEntity = await _userRepository.GetUserByEmail(
                userEmail: userEmail,
                cancellationToken: cancellationToken
            );
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with email: {userEmail}.",
                invalidEmail: userEmail,
                innerException: ex
            );
        }

        UserConfigurationEntity roleEntity = new UserConfigurationEntity
        {
            ConfigurationId = configurationId,
            Role = UserConfigRole.Member,
            UserId = userEntity.Id
        };

        await _userConfigurationRepository.AddUsersToConfiguration(
            entities: [roleEntity],
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task ChangeUserConfigurationRole(UserConfigurationRoleModel model, CancellationToken cancellationToken)
    {
        try
        {
            await ChangeUserConfigurationRoleUnsafe(model, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserDoesntBelongToConfigException(
                message:
                $"User with id: {model.UserId} doesnt belong to configuration with id: {model.ConfigurationId}",
                userId: model.UserId,
                configId: model.ConfigurationId,
                innerException: ex
            );
        }
    }

    private async Task ChangeUserConfigurationRoleUnsafe(UserConfigurationRoleModel model,
        CancellationToken cancellationToken)
    {
        using var transaction = _userConfigurationRepository.CreateTransactionScope();

        await _userConfigurationRepository.UpdateUserConfigRole(
            entity: new UserConfigurationEntity
            {
                UserId = model.UserId,
                Role = model.Role,
                ConfigurationId = model.ConfigurationId
            },
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task DeleteUserFromConfiguration(UserConfigurationRoleModel model, CancellationToken cancellationToken)
    {
        using var transaction = _userConfigurationRepository.CreateTransactionScope();

        await _userConfigurationRepository.DeleteUserFromConfiguration(
            userId: model.UserId,
            configurationId: model.ConfigurationId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task<IReadOnlyList<ConfigurationUsersViewModel>> GetConfigurationMembers(long configurationId,
        CancellationToken cancellationToken)
    {
        using var transaction = _userConfigurationRepository.CreateTransactionScope();
        
        var entities = await _userConfigurationRepository.GetConfigurationUsers(
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );
        
        transaction.Complete();

        return entities.MapEntitiesToModels();
    }
}