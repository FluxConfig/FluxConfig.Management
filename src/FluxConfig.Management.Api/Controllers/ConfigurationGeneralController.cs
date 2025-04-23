using FluxConfig.Management.Api.Contracts.Requests.Configurations.General;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.General;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Api.Mappers.Models;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations")]
public class ConfigurationGeneralController : ControllerBase
{
    private readonly IConfigurationsMetaService _configurationsMetaService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationGeneralController(IConfigurationsMetaService metaService,
        IRequestAuthContext requestAuthContext)
    {
        _configurationsMetaService = metaService;
        _requestAuthContext = requestAuthContext;
    }

    [HttpPost]
    [Route("create")]
    [Auth(RequiredRole = UserGlobalRole.Trusted)]
    [ProducesResponseType<CreateConfigurationResponse>(200)]
    [ErrorResponseType(400)] // TODO: Проверять только имя
    [ErrorResponseType(401)]
    public async Task<IActionResult> CreateNew(CreateConfigurationRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("change/name")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<ChangeConfigurationNameResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeName(ChangeConfigurationNameRequest request,
        CancellationToken cancellationToken)
    {
        await _configurationsMetaService.ChangeConfigurationName(
            changeNameModel: new ChangeConfigurationNameModel(
                NewName: request.NewName.Trim(),
                ConfigurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId
            ),
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeConfigurationNameResponse());
    }

    [HttpPatch]
    [Route("change/description")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<ChangeConfigurationDescriptionResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeDescription(ChangeConfigurationDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        await _configurationsMetaService.ChangeConfigurationDescription(
            configurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId,
            newDescription: request.NewDescription,
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeConfigurationDescriptionResponse());
    }

    [HttpDelete]
    [Route("delete")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<DeleteConfigurationResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("get")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<GetConfigurationMetaResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> GetMeta(CancellationToken cancellationToken)
    {
        UserConfigurationsViewModel model = await _configurationsMetaService.GetUserConfiguration(
            userId: _requestAuthContext.User!.Id,
            configurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId,
            cancellationToken: cancellationToken
        );

        return Ok(model.MapModelToMetaResponse());
    }


    [HttpGet]
    [Route("get-all")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<IEnumerable<GetUserConfigurationMetaResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<UserConfigurationsViewModel> models = await _configurationsMetaService.GetUserConfigurations(
            userId: _requestAuthContext.User!.Id,
            userRole: _requestAuthContext.User.Role,
            cancellationToken: cancellationToken
        );

        return Ok(models.MapModelsToResponsesAll());
    }
}