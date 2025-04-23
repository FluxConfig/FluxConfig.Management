using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationTagsService : IConfigurationTagsService
{
    private readonly IConfigurationTagsRepository _configurationTagsRepository;
    private readonly IConfigurationUsersService _configurationUsersService;

    public ConfigurationTagsService(
        IConfigurationTagsRepository configurationTagsRepository,
        IConfigurationUsersService configurationUsersService)
    {
        _configurationTagsRepository = configurationTagsRepository;
        _configurationUsersService = configurationUsersService;
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
}