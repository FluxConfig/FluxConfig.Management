using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;

public class RequestAuthContext: IRequestContext
{
    public UserModel? User { get; set; }
}