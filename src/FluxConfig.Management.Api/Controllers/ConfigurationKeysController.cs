using FluxConfig.Management.Api.Contracts.Requests.Configurations.Keys;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Keys;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations/keys")]
[Auth(RequiredRole = UserGlobalRole.Member)]
public class ConfigurationKeysController: ControllerBase
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
    [ErrorResponseType(404)] // TODO: Если конфигурации нет
    public async Task<IActionResult> Create(CreateConfigurationApiKeyRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("delete")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<DeleteConfigurationApiKeyResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> Delete(DeleteConfigurationApiKeyRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("get-all")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<IEnumerable<GetConfigurationApiKeyResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        // TODO: Получать только данные по праву ключи
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}