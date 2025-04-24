using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationsMetaService
{
    public Task<IReadOnlyList<UserConfigurationsViewModel>> GetUserConfigurations(long userId, UserGlobalRole userRole,
        CancellationToken cancellationToken);
    public Task ChangeConfigurationName(ChangeConfigurationNameModel changeNameModel ,CancellationToken cancellationToken);
    public Task ChangeConfigurationDescription(long configurationId, string newDescription,
        CancellationToken cancellationToken);
    public Task<UserConfigurationsViewModel> GetUserConfiguration(long userId, long configurationId, UserGlobalRole role,
        CancellationToken cancellationToken);
    public Task CreateNewConfiguration(ConfigurationModel model, long initiatorUserId, CancellationToken cancellationToken);
    public Task DeleteConfiguration(long configurationId, CancellationToken cancellationToken);
}