using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.KeyGen;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationKeysService : IConfigurationKeysService
{
    private readonly IConfigurationKeysRepository _configurationKeysRepository;
    private readonly IConfigurationTagsRepository _configurationTagsRepository;

    public ConfigurationKeysService(IConfigurationKeysRepository configurationKeysRepository,
        IConfigurationTagsRepository configurationTagsRepository)
    {
        _configurationKeysRepository = configurationKeysRepository;
        _configurationTagsRepository = configurationTagsRepository;
    }

    public async Task CreateNewKey(ConfigurationKeyModel keyModel, CancellationToken cancellationToken)
    {
        try
        {
            await CreateNewKeyUnsafe(keyModel, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationNotFoundException(
                message: $"Configuration with id: {keyModel.ConfigurationId} could not be found.",
                configurationId: keyModel.ConfigurationId,
                innerException: ex
            );
        }
    }

    private async Task CreateNewKeyUnsafe(ConfigurationKeyModel keyModel, CancellationToken cancellationToken)
    {
        ConfigurationKeyEntity entity = new ConfigurationKeyEntity
        {
            ConfigurationId = keyModel.ConfigurationId,
            Id = KeyGenerator.GenerateCompositeKey(),
            ExpirationDate = keyModel.ExpirationDate,
            RolePermission = keyModel.RolePermission
        };

        using var transaction = _configurationKeysRepository.CreateTransactionScope();

        await _configurationKeysRepository.AddKeys(
            entities: [entity],
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task DeleteKey(string keyId, long configurationId, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteKeyUnsafe(keyId, configurationId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationKeyNotFoundException(
                message: $"Configuration key with id: {keyId} could not be found.",
                keyId: keyId,
                innerException: ex
            );
        }
    }

    private async Task DeleteKeyUnsafe(string keyId, long configurationId, CancellationToken cancellationToken)
    {
        using var transaction = _configurationKeysRepository.CreateTransactionScope();

        await _configurationKeysRepository.DeleteKey(
            id: keyId,
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task<IReadOnlyList<ConfigurationKeyModel>> GetAllConfigKeysForRole(long configurationId,
        UserConfigRole role,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationKeysRepository.CreateTransactionScope();

        var configKeysEntities = await _configurationKeysRepository.GetAllForConfiguration(
            configurationId: configurationId,
            currentUtcTime: DateTimeOffset.UtcNow,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return configKeysEntities.MapEntitiesToModelsEnumerable().Where(m => m.RolePermission <= role).OrderBy(m => m.ExpirationDate).ToList();
    }

    public async Task<string> AuthenticateClientService(string apiKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        try
        {
            return await AuthenticateClientServiceUnsafe(apiKey, configurationTag, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new ConfigurationKeyNotFoundException(
                message: $"Configuration key with id: {apiKey} could not be authenticated.",
                keyId: apiKey,
                innerException: ex
            );
        }
    }

    private async Task<string> AuthenticateClientServiceUnsafe(string apiKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationKeysRepository.CreateTransactionScope();

        ConfigurationKeyEntity keyEntity = await _configurationKeysRepository.GetValidConfigurationKey(
            id: apiKey,
            curTimeUtc: DateTimeOffset.UtcNow,
            cancellationToken: cancellationToken
        );

        ConfigurationTagsViewEntity tagsViewEntity =
            await _configurationTagsRepository.GetTagWithConfigurationByConfigId(
                configurationId: keyEntity.ConfigurationId,
                tag: configurationTag,
                cancellationToken: cancellationToken
            );


        if (keyEntity.RolePermission < tagsViewEntity.RequiredRole)
        {
            throw new EntityNotFoundException($"Configuration key with id: {apiKey} could not be authenticated.");
        }

        transaction.Complete();

        return tagsViewEntity.StorageKey;
    }
}