using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class ConfigurationsMetaService : IConfigurationsMetaService
{
    private readonly IConfigurationsRepository _configurationsRepository;

    public ConfigurationsMetaService(IConfigurationsRepository configurationsRepository)
    {
        _configurationsRepository = configurationsRepository;
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

        return models;
    }
}