using FluxConfig.Management.Api.Contracts.Requests.Configurations.Tags;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Tags;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations/tags")]
[Auth(RequiredRole = UserGlobalRole.Member)]
public class ConfigurationTagsController : ControllerBase
{
    private readonly IConfigurationTagsService _configurationTagsService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationTagsController(IConfigurationTagsService configurationTagsService, IRequestAuthContext requestAuthContext)
    {
        _configurationTagsService = configurationTagsService;
        _requestAuthContext = requestAuthContext;
    }

    [HttpPost]
    [Route("create")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<AddConfigurationTagResponse>(200)]
    [ErrorResponseType(400)] // TODO: Проверяю сам
    [ErrorResponseType(401)]
    [ErrorResponseType(404)] // TODO: Если конфигурации нет
    [ErrorResponseType(409)] // TODO: Проверяю сам или получаю от storage
    public async Task<IActionResult> CreateTag(AddConfigurationTagRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("change/description")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<ChangeConfigurationTagDescriptionResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeDescription(ChangeConfigurationTagDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("change/role")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<ChangeConfigurationTagRoleResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeRequiredRole(ChangeConfigurationTagRoleRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("delete")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<DeleteConfigurationTagResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> Delete(DeleteConfigurationTagRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType<GetConfigurationTagMetaResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> GetMeta([FromQuery] GetConfigurationTagMetaRequest request,
        CancellationToken cancellationToken)
    {
        //TODO: валидировать внутри по требуемой роли тэга
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("get-all")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<IEnumerable<GetConfigurationTagResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        // TODO: Получать только данные по праву тэги
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}