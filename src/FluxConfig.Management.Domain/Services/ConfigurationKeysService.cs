using FluxConfig.Management.Domain.Contracts.Dal.Entities;
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

    public ConfigurationKeysService(IConfigurationKeysRepository configurationKeysRepository)
    {
        _configurationKeysRepository = configurationKeysRepository;
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

        await _configurationKeysRepository.AddKeys(
            entities: [entity],
            cancellationToken: cancellationToken
        );
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
        await _configurationKeysRepository.DeleteKey(
            id: keyId,
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );
    }

    public async Task<IReadOnlyList<ConfigurationKeyModel>> GetAllConfigKeysForRole(long configurationId,
        UserConfigRole role,
        CancellationToken cancellationToken)
    {
        var configKeysEntities = await _configurationKeysRepository.GetAllForConfiguration(
            configurationId: configurationId,
            currentUtcTime: DateTimeOffset.UtcNow,
            cancellationToken: cancellationToken
        );

        return configKeysEntities.mapEntitiesToModelsEnumerable().Where(m => m.RolePermission <= role).ToList();
    }
}