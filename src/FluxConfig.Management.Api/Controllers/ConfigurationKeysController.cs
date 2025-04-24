using FluxConfig.Management.Api.Contracts.Requests.Configurations.Keys;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Keys;
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
[Route("configurations/keys")]
[Auth(RequiredRole = UserGlobalRole.Member)]
public class ConfigurationKeysController : ControllerBase
{
    private readonly IConfigurationKeysService _configurationKeysService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationKeysController(IConfigurationKeysService keysService, IRequestAuthContext authContext)
    {
        _configurationKeysService = keysService;
        _requestAuthContext = authContext;
    }

    [HttpPost]
    [Route("create")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<CreateConfigurationApiKeyResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> Create(CreateConfigurationApiKeyRequest request,
        CancellationToken cancellationToken)
    {
        await _configurationKeysService.CreateNewKey(
            keyModel: request.MapRequestToModel(_requestAuthContext.ConfigurationRole!.ConfigurationId),
            cancellationToken: cancellationToken
        );

        return Ok(new CreateConfigurationApiKeyResponse());
    }

    [HttpDelete]
    [Route("delete")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<DeleteConfigurationApiKeyResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> Delete(DeleteConfigurationApiKeyRequest request,
        CancellationToken cancellationToken)
    {
        await _configurationKeysService.DeleteKey(
            keyId: request.Id.Trim(),
            configurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId,
            cancellationToken: cancellationToken
        );

        return Ok(new DeleteConfigurationApiKeyResponse());
    }

    [HttpGet]
    [Route("get-all")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<IEnumerable<GetConfigurationApiKeyResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var keysModels = await _configurationKeysService.GetAllConfigKeysForRole(
            configurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId,
            role: _requestAuthContext.ConfigurationRole.Role,
            cancellationToken: cancellationToken
        );

        return Ok(keysModels.MapModelsToResponses());
    }
}