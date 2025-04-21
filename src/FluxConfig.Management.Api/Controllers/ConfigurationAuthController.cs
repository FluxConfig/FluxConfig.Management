using FluxConfig.Management.Api.Contracts.Requests.Configurations.Auth;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Auth;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations/client-service")]
public class ConfigurationAuthController: ControllerBase
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
    public async Task<IActionResult> AuthClientService([FromQuery]ClientServiceAuthRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}