using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationKeysService
{
    public Task CreateNewKey(ConfigurationKeyModel keyModel, CancellationToken cancellationToken);
    public Task DeleteKey(string keyId, long configurationId, CancellationToken cancellationToken);
    public Task<IReadOnlyList<ConfigurationKeyModel>> GetAllConfigKeysForRole(long configurationId, UserConfigRole role, CancellationToken cancellationToken);
    public Task<string> AuthenticateClientService(string apiKey, string configurationTag,
        CancellationToken cancellationToken);
}