using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;

public interface IRequestAuthContext
{
    public UserModel? User { get; set; }
    public UserConfigurationRoleModel? ConfigurationRole { get; set; }
}