using FluentValidation;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Mappers.Configuration;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using FluxConfig.Management.Domain.Validators.Configurations;

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
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetUserConfigurationUnsafe(userId, configurationId, cancellationToken);
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
        CancellationToken cancellationToken)
    {
        using var transaction = _configurationsRepository.CreateTransactionScope();

        var userConfigEntity = await _configurationsRepository.GetUserConfigurationById(
            userId: userId,
            configurationId: configurationId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return userConfigEntity.MapEntityToModel();
    }
}