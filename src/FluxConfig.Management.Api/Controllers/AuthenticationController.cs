using FluxConfig.Management.Api.Contracts.Requests.Auth;
using FluxConfig.Management.Api.Contracts.Responses;
using FluxConfig.Management.Api.Contracts.Responses.Auth;
using FluxConfig.Management.Api.Mappers.Models;
using FluxConfig.Management.Api.Mappers.Requests;
using FluxConfig.Management.Domain.Models.Auth;
using FluxConfig.Management.Domain.Models.User;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserCredentialsService _userCredentialsService;

    public AuthenticationController(IUserCredentialsService credentialsService)
    {
        _userCredentialsService = credentialsService;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType<UserRegisterResponse>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [ProducesResponseType<ErrorResponse>(404)]
    [ProducesResponseType<ErrorResponse>(409)]
    public async Task<IActionResult> UserRegister(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        await _userCredentialsService.RegisterNewUser(
            model: request.MapRequestToModel(),
            cancellationToken: cancellationToken
        );

        return Ok(new UserRegisterResponse());
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType<UserLoginResponse>(200)]
    [ProducesResponseType<ErrorResponse>(404)]
    public async Task<IActionResult> UserLogin(UserLoginRequest request, CancellationToken cancellationToken)
    {
        SetCookieModel setCookieModel = await _userCredentialsService.LoginUser(
            model: request.MapRequestToModel(),
            cancellationToken: cancellationToken
        );

        Response.Cookies.Append(
            key: SessionModel.SessionCookieKey,
            value: setCookieModel.Session.Id,
            options: new CookieOptions
            {
                Expires = setCookieModel.Session.ExpirationDate,
                IsEssential = true,
                Domain = Request.Host.Host
            });

        return Ok(setCookieModel.MapModelToResponse());
    }

    [HttpGet]
    [Route("check-auth")]
    [ProducesResponseType<UserCheckAuthResponse>(200)]
    [ProducesResponseType<ErrorResponse>(401)]
    [ProducesResponseType<ErrorResponse>(404)]
    public async Task<IActionResult> UserCheckAuth(CancellationToken cancellationToken)
    {
        UserModel userModel = await _userCredentialsService.UserCheckAuth(
            sessionId: HttpContext.Request.Cookies[SessionModel.SessionCookieKey],
            cancellationToken: cancellationToken
        );

        return Ok(userModel.MapModelToResponse());
    }

    [HttpDelete]
    [Route("logout")]
    [ProducesResponseType<UserLogoutResponse>(200)]
    public async Task<IActionResult> UserLogout(CancellationToken cancellationToken)
    {
        await _userCredentialsService.LogoutUser(
            sessionId: HttpContext.Request.Cookies[SessionModel.SessionCookieKey] ?? "",
            cancellationToken: cancellationToken
        );

        Response.Cookies.Delete(
            key: SessionModel.SessionCookieKey,
            options: new CookieOptions
            {
                IsEssential = true,
                Domain = Request.Host.Host
            }
        );

        return Ok(new UserLogoutResponse());
    }
}