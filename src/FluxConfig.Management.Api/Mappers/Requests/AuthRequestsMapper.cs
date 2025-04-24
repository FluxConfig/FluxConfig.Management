using FluxConfig.Management.Api.Contracts.Requests.Auth;
using FluxConfig.Management.Domain.Models.Auth;

namespace FluxConfig.Management.Api.Mappers.Requests;

internal static class AuthRequestsMapper
{
    internal static UserRegisterModel MapRequestToModel(this UserRegisterRequest request)
    {
        return new UserRegisterModel(
            Username: NullOrTrim(request.Username),
            Email: NullOrTrim(request.Email),
            Password: NullOrTrim(request.Password)
        );
    }

    internal static UserLoginModel MapRequestToModel(this UserLoginRequest request)
    {
        return new UserLoginModel(
            Email: NullOrTrim(request.Email),
            Password: NullOrTrim(request.Password),
            RememberUser: request.RememberUser ?? false
        );
    }

    private static string NullOrTrim(string? checkString)
    {
        return checkString == null ? "" : checkString.Trim();
    }
}