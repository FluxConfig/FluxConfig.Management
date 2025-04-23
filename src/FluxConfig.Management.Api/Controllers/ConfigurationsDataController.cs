using FluxConfig.Management.Api.Contracts.Requests.Configurations.Data;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Data;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Api.Mappers.Models;
using FluxConfig.Management.Api.Mappers.Requests;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations/data")]
[Auth(RequiredRole = UserGlobalRole.Member)]
public class ConfigurationsDataController : ControllerBase
{
    private readonly IConfigurationsDataService _configurationsDataService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationsDataController(IConfigurationsDataService dataService, IRequestAuthContext requestAuthContext)
    {
        _configurationsDataService = dataService;
        _requestAuthContext = requestAuthContext;
    }

    [HttpGet]
    [Route("load")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<IEnumerable<ConfigurationKeyValueResponse>>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> LoadConfigData([FromQuery]LoadConfigDataRequest request, CancellationToken cancellationToken)
    {
        var models = await _configurationsDataService.LoadConfigurationData(
            tagId: request.TagId,
            userRole: _requestAuthContext.ConfigurationRole!.Role,
            dataType: request.DataType,
            cancellationToken: cancellationToken
        );

        return Ok(models.MapModelsToResponses());
    }

    [HttpPost]
    [Route("update")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<UpdateConfigurationDataResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> UpdateConfigData(UpdateConfigurationDataRequest request,
        CancellationToken cancellationToken)
    {
        await _configurationsDataService.UpdateConfigurationData(
            tagId: request.TagId,
            dataType: request.DataType,
            userRole: _requestAuthContext.ConfigurationRole!.Role,
            updatedData: request.Data.MapRequestsToModels(),
            cancellationToken: cancellationToken
        );

        return Ok(new UpdateConfigurationDataResponse());
    }
}