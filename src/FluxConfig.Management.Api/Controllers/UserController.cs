using FluxConfig.Management.Api.Contracts.Requests.User;
using FluxConfig.Management.Api.Contracts.Responses.User;
using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Api.Mappers.Models;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Models.User;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
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
        await _userService.ChangeUserEmail(
            changeEmailModel: new ChangeUserEmailModel(
                User: _requestAuthContext.User!,
                NewEmail: NullOrTrim(request.NewEmail)
            ),
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeUserEmailResponse());
    }

    [HttpPatch]
    [Route("change/password")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<ChangeUserPasswordResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordRequest request,
        CancellationToken cancellationToken)
    {
        await _userService.ChangeUserPassword(
            changePasswordModel: new ChangeUserPasswordModel(
                User: _requestAuthContext.User!,
                NewPassword: NullOrTrim(request.NewPassword)
            ),
            cancellationToken: cancellationToken
        );


        return Ok(new ChangeUserPasswordResponse());
    }

    [HttpPatch]
    [Route("change/username")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<ChangeUserUsernameResponse>(200)]
    [ErrorResponseType(400)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeUsername(ChangeUserUsernameRequest request,
        CancellationToken cancellationToken)
    {
        await _userService.ChangeUserUsername(
            changeUsernameModel: new ChangeUserUsernameModel(
                User: _requestAuthContext.User!,
                NewUsername: NullOrTrim(request.NewUsername)
            ),
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeUserUsernameResponse());
    }

    [HttpDelete]
    [Route("delete")]
    [Auth(RequiredRole = UserGlobalRole.Member)]
    [ProducesResponseType<DeleteAccountResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> DeleteAccount(CancellationToken cancellationToken)
    {
        await _userService.DeleteUser(
            userId: _requestAuthContext.User!.Id,
            cancellationToken: cancellationToken
        );

        return Ok(new DeleteAccountResponse());
    }

    # endregion

    # region Admin

    [HttpDelete]
    [Route("admin/delete")]
    [Auth(RequiredRole = UserGlobalRole.Admin)]
    [ProducesResponseType<DeleteUserByAdminResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> DeleteUserByAdmin(DeleteUserByAdminRequest request,
        CancellationToken cancellationToken)
    {
        await _userService.DeleteUser(
            userId: request.UserId,
            cancellationToken: cancellationToken
        );

        return Ok(new DeleteUserByAdminResponse());
    }

    [HttpGet]
    [Route("admin/get-users")]
    [Auth(RequiredRole = UserGlobalRole.Admin)]
    [ProducesResponseType<IEnumerable<GetSystemUserResponse>>(200)]
    [ErrorResponseType(401)]
    public async Task<IActionResult> GetSystemUsers(CancellationToken cancellationToken)
    {
        IReadOnlyList<UserModel> systemUsers = await _userService.GetAllUsers(cancellationToken);

        return Ok(systemUsers.MapModelsToSysResponses());
    }

    [HttpPatch]
    [Route("admin/change-user-role")]
    [Auth(RequiredRole = UserGlobalRole.Admin)]
    [ProducesResponseType<ChangeUserRoleResponse>(200)]
    [ErrorResponseType(401)]
    [ErrorResponseType(404)]
    public async Task<IActionResult> ChangeUserRole(ChangeUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        await _userService.ChangeUserRole(
            userId: request.UserId,
            newRole: request.Role,
            cancellationToken: cancellationToken
        );

        return Ok(new ChangeUserRoleResponse());
    }

    # endregion

    private static string NullOrTrim(string? checkString)
    {
        return checkString == null ? "" : checkString.Trim();
    }
}