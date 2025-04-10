using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;

public interface IRequestContext
{
    public UserModel? User { get; set; }
}