using FluentValidation;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Domain.Validators.Configurations;

public class ConfigurationModelValidator: AbstractValidator<ConfigurationModel>
{
    public ConfigurationModelValidator()
    {
        RuleFor(m => m.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(m => m.Description)
            .NotNull()
            .MaximumLength(500);
    }
}