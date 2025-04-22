using FluxConfig.Management.Api.Contracts.Requests.Configurations.Auth;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Auth;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations/client-service")]
public class ConfigurationAuthController : ControllerBase
{
    private readonly IConfigurationKeysService _configurationKeysService;

    public ConfigurationAuthController(IConfigurationKeysService keysService)
    {
        _configurationKeysService = keysService;
    }

    [HttpGet]
    [Route("auth")]
    [ProducesResponseType<ClientServiceAuthResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> AuthClientService([FromQuery] ClientServiceAuthRequest request,
        CancellationToken cancellationToken)
    {
        string storageKey = await _configurationKeysService.AuthenticateClientService(
            apiKey: NullOrTrim(request.ApiKey),
            configurationTag: NullOrTrim(request.Tag),
            cancellationToken: cancellationToken
        );

        return Ok(new ClientServiceAuthResponse(
            ConfigKey: storageKey
        ));
    }

    private static string NullOrTrim(string? val)
    {
        return val == null ? "" : val.Trim();
    }
}