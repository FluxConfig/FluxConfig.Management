using FluentValidation;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Contracts.ISC.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using FluxConfig.Management.Domain.Validators.Configurations;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationTagsService : IConfigurationTagsService
{
    private readonly IConfigurationTagsRepository _configurationTagsRepository;
    private readonly IConfigurationsRepository _configurationsRepository;
    private readonly IConfigurationUsersService _configurationUsersService;
    private readonly IFluxConfigStorageClient _client;

    public ConfigurationTagsService(
        IConfigurationTagsRepository configurationTagsRepository,
        IConfigurationsRepository configurationsRepository,
        IConfigurationUsersService configurationUsersService,
        IFluxConfigStorageClient storageClient)
    {
        _configurationTagsRepository = configurationTagsRepository;
        _configurationsRepository = configurationsRepository;
        _configurationUsersService = configurationUsersService;
        _client = storageClient;
    }

    public async Task ChangeTagDescription(long tagId, string newDescription, CancellationToken cancellationToken)
    {
        try
        {
            await ChangeTagDescriptionUnsafe(tagId, newDescription, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationTagNotFoundException(
                message: $"Configuration tag with id: {tagId} couldn't be found.",
                tagId: tagId,
                innerException: ex
            );
        }
    }

    private async Task ChangeTagDescriptionUnsafe(long tagId, string newDescription,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationTagsRepository.CreateTransactionScope();

        await _configurationTagsRepository.UpdateTagDescription(
            tagId: tagId,
            newDescription: newDescription,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task ChangeTagRequiredRole(long tagId, UserConfigRole newRole, CancellationToken cancellationToken)
    {
        try
        {
            await ChangeTagRequiredRoleUnsafe(tagId, newRole, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationTagNotFoundException(
                message: $"Configuration tag with id: {tagId} couldn't be found.",
                tagId: tagId,
                innerException: ex
            );
        }
    }

    private async Task ChangeTagRequiredRoleUnsafe(long tagId, UserConfigRole newRole,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationTagsRepository.CreateTransactionScope();

        await _configurationTagsRepository.UpdateTagRequiredRole(
            tagId: tagId,
            newRole: newRole,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task<IReadOnlyList<ConfigurationTagModel>> GetConfigurationTags(long configurationId,
        UserConfigRole userRole, CancellationToken cancellationToken)
    {
        using var transaction = _configurationTagsRepository.CreateTransactionScope();

        IReadOnlyList<ConfigurationTagEntity> entities = await _configurationTagsRepository.GetConfigurationTags(
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return entities.MapEntitiesToModels().Where(m => m.RequiredRole <= userRole).ToList();
    }

    public async Task<ConfigurationTagsViewModel> GetTagMeta(
        long tagId,
        long userId,
        UserGlobalRole userRole,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetTagMetaUnsafe(tagId, userId, userRole, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationTagNotFoundException(
                message: $"Configuration tag with id: {tagId} couldn't be found.",
                tagId: tagId,
                innerException: ex
            );
        }
    }

    private async Task<ConfigurationTagsViewModel> GetTagMetaUnsafe(
        long tagId,
        long userId,
        UserGlobalRole userRole,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationTagsRepository.CreateTransactionScope();

        ConfigurationTagsViewEntity tagEntity = await _configurationTagsRepository.GetTagWithConfigurationByTagId(
            tagId: tagId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        if (userRole != UserGlobalRole.Admin)
        {
            await _configurationUsersService.CheckUserConfigPermissions(
                userId: userId,
                configurationId: tagEntity.ConfigurationId,
                requiredRole: tagEntity.RequiredRole,
                cancellationToken: cancellationToken
            );
        }

        return tagEntity.MapEntityToModel();
    }

    public async Task CreateTag(ConfigurationTagModel model, CancellationToken cancellationToken)
    {
        try
        {
            await CreateTagUnsafe(model, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {model.ConfigurationId} couldn't be found.",
                configurationId: model.ConfigurationId,
                innerException: ex
            );
        }
        catch (EntityAlreadyExistsException ex)
        {
            throw new ConfigurationTagAlreadyExistsException(
                message:
                $"Configuration tag: {model.ConfigurationId} already exists for configuration with id: {model.ConfigurationId}",
                configurationId: model.ConfigurationId,
                tag: model.Tag,
                innerException: ex
            );
        }
        catch (FcStorageAlreadyExistsException ex)
        {
            throw new ConfigurationTagAlreadyExistsException(
                message:
                $"Configuration tag: {model.ConfigurationId} already exists for configuration with id: {model.ConfigurationId}",
                configurationId: model.ConfigurationId,
                tag: ex.Tag,
                innerException: ex
            );
        }
    }

    private async Task CreateTagUnsafe(ConfigurationTagModel model, CancellationToken cancellationToken)
    {
        var validator = new ConfigurationTagModelValidator();
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        using var transaction = _configurationsRepository.CreateTransactionScope();

        ConfigurationEntity configEntity = await _configurationsRepository.GetConfigurationById(
            configurationId: model.ConfigurationId,
            cancellationToken: cancellationToken
        );

        ConfigurationTagEntity newEntity = new ConfigurationTagEntity
        {
            ConfigurationId = configEntity.Id,
            Description = model.Description,
            Id = -1,
            RequiredRole = model.RequiredRole,
            Tag = model.Tag
        };

        await _configurationTagsRepository.CreateConfigurationTags(
            entities: [newEntity],
            cancellationToken: cancellationToken
        );

        await _client.CreateConfiguration(
            key: configEntity.StorageKey,
            tag: model.Tag,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task DeleteTag(long tagId, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteTagUnsafe(tagId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationTagNotFoundException(
                message: $"Configuration tag with id: {tagId} couldn't be found.",
                tagId: tagId,
                innerException: ex
            );
        }
        catch (FcStorageNotFoundException ex)
        {
            throw new ConfigurationTagNotFoundException(
                message: $"Configuration tag with id: {tagId} couldn't be found.",
                tagId: tagId,
                innerException: ex
            );
        }
    }

    private async Task DeleteTagUnsafe(long tagId, CancellationToken cancellationToken)
    {
        using var transaction = _configurationsRepository.CreateTransactionScope();

        ConfigurationTagsViewEntity tagsViewEntity =
            await _configurationTagsRepository.GetTagWithConfigurationByTagId(
                tagId: tagId,
                cancellationToken: cancellationToken
            );

        await _configurationTagsRepository.DeleteConfigurationTag(
            tagId: tagId,
            cancellationToken: cancellationToken
        );

        await _client.DeleteConfiguration(
            key: tagsViewEntity.StorageKey,
            tags: [tagsViewEntity.TagName],
            cancellationToken: cancellationToken
            );

        transaction.Complete();
    }
}