using FluentValidation;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Domain.Validators.Configurations;

public class ConfigurationTagModelValidator: AbstractValidator<ConfigurationTagModel>
{
    public ConfigurationTagModelValidator()
    {
        RuleFor(m => m.Tag)
            .NotNull()
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(m => m.Description)
            .NotNull()
            .MaximumLength(500);
    }
}