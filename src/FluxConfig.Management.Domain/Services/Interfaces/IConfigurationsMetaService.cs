using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationsMetaService
{
    public Task<IReadOnlyList<UserConfigurationsViewModel>> GetUserConfigurations(long userId, UserGlobalRole userRole,
        CancellationToken cancellationToken);
}