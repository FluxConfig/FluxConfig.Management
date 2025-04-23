using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Contracts.ISC.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationsDataService : IConfigurationsDataService
{
    private readonly IConfigurationTagsRepository _configurationTagsRepository;
    private readonly IFluxConfigStorageClient _client;

    public ConfigurationsDataService(IConfigurationTagsRepository configurationTagsRepository,
        IFluxConfigStorageClient storageClient)
    {
        _configurationTagsRepository = configurationTagsRepository;
        _client = storageClient;
    }

    public async Task UpdateConfigurationData(long tagId, UserConfigRole userRole, ConfigurationDataType dataType,
        IReadOnlyList<ConfigurationKeyValueType> updatedData, CancellationToken cancellationToken)
    {
        try
        {
            await UpdateConfigurationDataUnsafe(tagId, userRole, dataType, updatedData, cancellationToken);
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

    private async Task UpdateConfigurationDataUnsafe(long tagId, UserConfigRole userRole,
        ConfigurationDataType dataType,
        IReadOnlyList<ConfigurationKeyValueType> updatedData, CancellationToken cancellationToken)
    {
        using var transaction = _configurationTagsRepository.CreateTransactionScope();

        ConfigurationTagsViewEntity tagEntity = await _configurationTagsRepository.GetTagWithConfigurationByTagId(
            tagId: tagId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        if (userRole < tagEntity.RequiredRole)
        {
            throw new UserConfigUnauthorizedException(
                message: "Dont have enough permissions to access resource.",
                reason: $"Required role: {tagEntity.RequiredRole.ToString()}"
            );
        }

        await _client.UpdateConfigData(
            key: tagEntity.StorageKey,
            tag: tagEntity.TagName,
            dataType: dataType,
            updatedData: updatedData,
            cancellationToken: cancellationToken
        );
    }

    public async Task<IReadOnlyList<ConfigurationKeyValueType>> LoadConfigurationData(long tagId,
        UserConfigRole userRole, ConfigurationDataType dataType,
        CancellationToken cancellationToken)
    {
        try
        {
            return await LoadConfigurationDataUnsafe(tagId, userRole, dataType, cancellationToken);
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

    private async Task<IReadOnlyList<ConfigurationKeyValueType>> LoadConfigurationDataUnsafe(long tagId,
        UserConfigRole userRole, ConfigurationDataType dataType,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationTagsRepository.CreateTransactionScope();

        ConfigurationTagsViewEntity tagEntity = await _configurationTagsRepository.GetTagWithConfigurationByTagId(
            tagId: tagId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        if (userRole < tagEntity.RequiredRole)
        {
            throw new UserConfigUnauthorizedException(
                message: "Dont have enough permissions to access resource.",
                reason: $"Required role: {tagEntity.RequiredRole.ToString()}"
            );
        }

        var configData = await _client.LoadConfigData(
            key: tagEntity.StorageKey,
            tag: tagEntity.TagName,
            dataType: dataType,
            cancellationToken: cancellationToken
        );

        return configData.OrderBy(m => m.Key).ToList();
    }
}