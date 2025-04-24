using FluxConfig.Management.Api.Contracts.Requests.Configurations.Tags;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Tags;
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
[Route("configurations/tags")]
[Auth(RequiredRole = UserGlobalRole.Member)]
public class ConfigurationTagsController : ControllerBase
{
    private readonly IConfigurationTagsService _configurationTagsService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationTagsController(IConfigurationTagsService configurationTagsService,
        IRequestAuthContext requestAuthContext)
    {
        _configurationTagsService = configurationTagsService;
        _requestAuthContext = requestAuthContext;
    }

    [HttpPost]
    [Route("create")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<AddConfigurationTagResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)] 
    [ErrorResponseType(409)]
    public async Task<IActionResult> CreateTag(AddConfigurationTagRequest request, CancellationToken cancellationToken)
    {
        await _configurationTagsService.CreateTag(
            model: new ConfigurationTagModel(
                Id: -1,
                ConfigurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId,
                Tag: request.Tag,
                Description: request.Description,
                RequiredRole: request.RequiredRole
            ),
            cancellationToken: cancellationToken
        );

        return Ok(new AddConfigurationTagResponse());
    }

    [HttpPatch]
    [Route("change/description")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<ChangeConfigurationTagDescriptionResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeDescription(ChangeConfigurationTagDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        await _configurationTagsService.ChangeTagDescription(
            tagId: request.TagId,
            newDescription: request.NewDescription,
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeConfigurationTagDescriptionResponse());
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
        await _configurationTagsService.ChangeTagRequiredRole(
            tagId: request.TagId,
            newRole: request.NewRole,
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeConfigurationTagRoleResponse());
    }

    [HttpDelete]
    [Route("delete")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<DeleteConfigurationTagResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> Delete(DeleteConfigurationTagRequest request, CancellationToken cancellationToken)
    {
        await _configurationTagsService.DeleteTag(
            tagId: request.TagId,
            cancellationToken: cancellationToken
        );

        return Ok(new DeleteConfigurationTagResponse());
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType<GetConfigurationTagMetaResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> GetMeta([FromQuery] GetConfigurationTagMetaRequest request,
        CancellationToken cancellationToken)
    {
        ConfigurationTagsViewModel model = await _configurationTagsService.GetTagMeta(
            tagId: request.TagId,
            userId: _requestAuthContext.User!.Id,
            userRole: _requestAuthContext.User.Role,
            cancellationToken: cancellationToken
        );

        return Ok(model.MapModelToMetaResponse());
    }

    [HttpGet]
    [Route("get-all")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<IEnumerable<GetConfigurationTagResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var models = await _configurationTagsService.GetConfigurationTags(
            configurationId: _requestAuthContext.ConfigurationRole!.ConfigurationId,
            userRole: _requestAuthContext.ConfigurationRole!.Role,
            cancellationToken: cancellationToken
        );

        return Ok(models.MapModelsToResponses());
    }
}