using FluentValidation;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Contracts.ISC.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using FluxConfig.Management.Domain.KeyGen;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using FluxConfig.Management.Domain.Validators.Configurations;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationsMetaService : IConfigurationsMetaService
{
    private readonly IConfigurationsRepository _configurationsRepository;
    private readonly IFluxConfigStorageClient _client;
    private readonly IConfigurationTagsRepository _configurationTagsRepository;
    private readonly IUserConfigurationRepository _userConfigurationRepository;
    private readonly IConfigurationKeysRepository _configurationKeysRepository;


    public ConfigurationsMetaService(
        IConfigurationsRepository configurationsRepository,
        IConfigurationTagsRepository configurationTagsRepository,
        IUserConfigurationRepository userConfigurationRepository,
        IConfigurationKeysRepository configurationKeysRepository,
        IFluxConfigStorageClient storageClient
    )
    {
        _configurationsRepository = configurationsRepository;
        _configurationTagsRepository = configurationTagsRepository;
        _userConfigurationRepository = userConfigurationRepository;
        _configurationKeysRepository = configurationKeysRepository;
        _client = storageClient;
    }

    public async Task<IReadOnlyList<UserConfigurationsViewModel>> GetUserConfigurations(
        long userId,
        UserGlobalRole userRole,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationsRepository.CreateTransactionScope();

        IReadOnlyList<UserConfigurationsViewModel> models;
        if (userRole == UserGlobalRole.Admin)
        {
            var configurationEntities = await _configurationsRepository.GetAllConfigurations(cancellationToken);

            models = configurationEntities.Select(e => new UserConfigurationsViewModel(
                UserId: userId,
                ConfigurationId: e.Id,
                Role: UserConfigRole.Admin,
                ConfigurationName: e.Name,
                ConfigurationDescription: e.Description
            )).ToList();
        }
        else
        {
            var userConfigurationEntities = await _configurationsRepository.GetUserConfigurations(
                userId: userId,
                cancellationToken: cancellationToken
            );

            models = userConfigurationEntities.MapEntitiesToModels();
        }

        transaction.Complete();

        return models.OrderBy(m => m.ConfigurationName).ToList();
    }

    public async Task ChangeConfigurationName(ChangeConfigurationNameModel changeNameModel,
        CancellationToken cancellationToken)
    {
        try
        {
            await ChangeConfigurationNameUnsafe(changeNameModel, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {changeNameModel.ConfigurationId} couldn't be found.",
                configurationId: changeNameModel.ConfigurationId,
                innerException: ex
            );
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
    }

    private async Task ChangeConfigurationNameUnsafe(ChangeConfigurationNameModel changeNameModel,
        CancellationToken cancellationToken)
    {
        var validator = new ChangeConfigurationNameModelValidator();
        await validator.ValidateAndThrowAsync(changeNameModel, cancellationToken);

        using var transaction = _configurationsRepository.CreateTransactionScope();

        await _configurationsRepository.ChangeConfigurationName(
            id: changeNameModel.ConfigurationId,
            newName: changeNameModel.NewName,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task ChangeConfigurationDescription(long configurationId, string newDescription,
        CancellationToken cancellationToken)
    {
        try
        {
            await ChangeConfigurationDescriptionUnsafe(configurationId, newDescription, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {configurationId} couldn't be found.",
                configurationId: configurationId,
                innerException: ex
            );
        }
    }

    private async Task ChangeConfigurationDescriptionUnsafe(long configurationId, string newDescription,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationsRepository.CreateTransactionScope();

        await _configurationsRepository.ChangeConfigurationDescription(
            id: configurationId,
            newDescription: newDescription,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task<UserConfigurationsViewModel> GetUserConfiguration(long userId, long configurationId,
        UserGlobalRole role,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetUserConfigurationUnsafe(userId, configurationId, role, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {configurationId} couldn't be found.",
                configurationId: configurationId,
                innerException: ex
            );
        }
    }

    private async Task<UserConfigurationsViewModel> GetUserConfigurationUnsafe(long userId, long configurationId,
        UserGlobalRole role,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationsRepository.CreateTransactionScope();

        if (role == UserGlobalRole.Admin)
        {
            ConfigurationEntity configEntity = await _configurationsRepository.GetConfigurationById(
                configurationId: configurationId,
                cancellationToken: cancellationToken
            );

            return new UserConfigurationsViewModel(
                UserId: userId,
                ConfigurationId: configurationId,
                Role: UserConfigRole.Admin,
                ConfigurationName: configEntity.Name,
                ConfigurationDescription: configEntity.Description
            );
        }

        var userConfigEntity = await _configurationsRepository.GetUserConfigurationById(
            userId: userId,
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return userConfigEntity.MapEntityToModel();
    }

    public async Task CreateNewConfiguration(ConfigurationModel model, long initiatorUserId,
        CancellationToken cancellationToken)
    {
        try
        {
            await CreateNewConfigurationUnsafe(model, initiatorUserId, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
    }

    private async Task CreateNewConfigurationUnsafe(ConfigurationModel model, long initiatorUserId,
        CancellationToken cancellationToken)
    {
        var validator = new ConfigurationModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        string storageKey = KeyGenerator.GenerateCompositeKey();

        ConfigurationEntity creatingEntity = new ConfigurationEntity()
        {
            Description = model.Description,
            Name = model.Name,
            StorageKey = storageKey
        };

        using var transaction = _configurationsRepository.CreateTransactionScope();

        // configuration
        long configurationId = await _configurationsRepository.Create(
            entity: creatingEntity,
            cancellationToken: cancellationToken
        );

        // master and member key
        await CreateNewConfigurationApiKeys(configurationId, cancellationToken);

        // initiator user
        await CreateNewConfigurationAdminUser(configurationId, initiatorUserId, cancellationToken);

        // development and prod tags
        await CreateNewConfigurationTags(configurationId, storageKey, cancellationToken);

        transaction.Complete();
    }

    private async Task CreateNewConfigurationApiKeys(long configurationId, CancellationToken cancellationToken)
    {
        ConfigurationKeyEntity masterKeyEntity = new ConfigurationKeyEntity
        {
            ConfigurationId = configurationId,
            Id = KeyGenerator.GenerateCompositeKey(),
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(1),
            RolePermission = UserConfigRole.Admin
        };

        ConfigurationKeyEntity memberKeyEntity = new ConfigurationKeyEntity
        {
            ConfigurationId = configurationId,
            Id = KeyGenerator.GenerateCompositeKey(),
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(1),
            RolePermission = UserConfigRole.Member
        };

        await _configurationKeysRepository.AddKeys(
            entities: [masterKeyEntity, memberKeyEntity],
            cancellationToken: cancellationToken
        );
    }

    private async Task CreateNewConfigurationAdminUser(long configurationId, long initiatorUserId,
        CancellationToken cancellationToken)
    {
        UserConfigurationEntity adminEntity = new UserConfigurationEntity
        {
            ConfigurationId = configurationId,
            Role = UserConfigRole.Admin,
            UserId = initiatorUserId
        };

        await _userConfigurationRepository.AddUsersToConfiguration(
            entities: [adminEntity],
            cancellationToken: cancellationToken
        );
    }

    private async Task CreateNewConfigurationTags(long configurationId, string storageKey,
        CancellationToken cancellationToken)
    {
        ConfigurationTagEntity prodTag = new ConfigurationTagEntity
        {
            ConfigurationId = configurationId,
            Description = "Version of the configuration for usage in the production environment.",
            RequiredRole = UserConfigRole.Admin,
            Tag = "Production"
        };

        ConfigurationTagEntity envTag = new ConfigurationTagEntity
        {
            ConfigurationId = configurationId,
            Description = "Version of the configuration for usage in the development environment.",
            RequiredRole = UserConfigRole.Member,
            Tag = "Development"
        };

        await _configurationTagsRepository.CreateConfigurationTags(
            entities: [prodTag, envTag],
            cancellationToken: cancellationToken
        );

        // storage call
        await _client.CreateConfiguration(
            key: storageKey,
            tag: "Production",
            cancellationToken: cancellationToken
        );

        await _client.CreateConfiguration(
            key: storageKey,
            tag: "Development",
            cancellationToken: cancellationToken
        );
    }


    public async Task DeleteConfiguration(long configurationId, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteConfigurationUnsafe(configurationId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {configurationId} couldn't be found.",
                configurationId: configurationId,
                innerException: ex
            );
        }
        catch (FcStorageNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {configurationId} couldn't be found.",
                configurationId: configurationId,
                innerException: ex
            );
        }
    }

    private async Task DeleteConfigurationUnsafe(long configurationId, CancellationToken cancellationToken)
    {
        using var transaction = _configurationsRepository.CreateTransactionScope();

        IReadOnlyList<ConfigurationTagEntity> configurationTags =
            await _configurationTagsRepository.GetConfigurationTags(
                configurationId: configurationId,
                cancellationToken: cancellationToken
            );

        List<string> deletingTags = configurationTags.Select(e => e.Tag).ToList();

        ConfigurationEntity deletingConfiguration = await _configurationsRepository.GetConfigurationById(
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        string deletingStorageKey = deletingConfiguration.StorageKey;

        await _configurationsRepository.Delete(
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        await _client.DeleteConfiguration(
            key: deletingStorageKey,
            tags: deletingTags,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }
}