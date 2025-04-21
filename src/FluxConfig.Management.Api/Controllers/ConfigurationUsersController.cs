using FluxConfig.Management.Api.Contracts.Requests.Configurations.Users;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Users;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("configurations/user")]
[Auth(RequiredRole = UserGlobalRole.Member)]
public class ConfigurationUsersController : ControllerBase
{
    private readonly IConfigurationUsersService _configurationUsersService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationUsersController(IConfigurationUsersService usersService, IRequestAuthContext authContext)
    {
        _configurationUsersService = usersService;
        _requestAuthContext = authContext;
    }

    [HttpPost]
    [Route("add-to-config")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<AddUserToConfigurationResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> AddToConfiguration(AddUserToConfigurationRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpPatch]
    [Route("change-role")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<ChangeUserConfigurationRoleResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeRole(ChangeUserConfigurationRoleRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("delete")]
    [ConfigAuth(RequiredRole = UserConfigRole.Admin)]
    [ProducesResponseType<DeleteUserFromConfigurationResponse>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> DeleteUser(DeleteUserFromConfigurationRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("get-all")]
    [ConfigAuth(RequiredRole = UserConfigRole.Member)]
    [ProducesResponseType<IEnumerable<GetConfigurationUserResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetConfigurationUsers(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}