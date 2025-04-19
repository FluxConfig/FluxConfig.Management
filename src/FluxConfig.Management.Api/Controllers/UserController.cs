using FluxConfig.Management.Api.Contracts.Requests.User;
using FluxConfig.Management.Api.Contracts.Responses.User;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRequestContext _requestAuthContext;

    public UserController(IUserService userService, IRequestContext requestAuthContext)
    {
        _userService = userService;
        _requestAuthContext = requestAuthContext;
    }
    
    # region Member
    
    [HttpPatch]
    [Route("change/email")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<ChangeUserEmailResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    [ErrorResponseType(409)]
    public async Task<IActionResult> ChangeEmail(ChangeUserEmailRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new ChangeUserEmailResponse());
    }
    
    [HttpPatch]
    [Route("change/password")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<ChangeUserPasswordResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new ChangeUserPasswordResponse());
    }
    
    [HttpPatch]
    [Route("change/username")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<ChangeUserUsernameResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeUsername(ChangeUserUsernameRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new ChangeUserUsernameResponse());
    }

    [HttpDelete]
    [Route("delete")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<DeleteUserResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new DeleteUserResponse());
    }
    
    # endregion
    
    # region Admin
    
    [HttpDelete]
    [Route("admin/delete")]
    [Auth(RequiredRole = UserGlobalRole.Admin)]
    [ProducesResponseType<DeleteUserByAdminResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> DeleteUserByAdmin(DeleteUserByAdminRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new DeleteUserByAdminResponse());
    }

    [HttpGet]
    [Route("admin/get-all")]
    [Auth(RequiredRole = UserGlobalRole.Admin)]
    [ProducesResponseType<IEnumerable<GetSystemUserResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetSystemUsers(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new List<GetSystemUserResponse>());
    }

    [HttpPatch]
    [Route("admin/change-role")]
    [Auth(RequiredRole = UserGlobalRole.Admin)]
    [ProducesResponseType<ChangeUserRoleResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeUserRole(ChangeUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        return Ok(new ChangeUserRoleResponse());
    }
    
    # endregion
}