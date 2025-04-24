using FluentValidation;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Domain.Validators.Configurations;

public class ChangeConfigurationNameModelValidator: AbstractValidator<ChangeConfigurationNameModel>
{
    public ChangeConfigurationNameModelValidator()
    {
        RuleFor(m => m.NewName)
            .NotNull()
            .NotEmpty();
    }
}