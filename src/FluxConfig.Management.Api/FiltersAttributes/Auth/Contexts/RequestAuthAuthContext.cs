using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;

public class RequestAuthAuthContext: IRequestAuthContext
{
    public UserModel? User { get; set; }
    public UserConfigurationRoleModel? ConfigurationRole { get; set; }
}