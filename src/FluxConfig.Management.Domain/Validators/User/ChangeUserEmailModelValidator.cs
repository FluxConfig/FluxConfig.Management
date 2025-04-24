using FluentValidation;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Validators.User;

public class ChangeUserEmailModelValidator : AbstractValidator<ChangeUserEmailModel>
{
    public ChangeUserEmailModelValidator()
    {
        RuleFor(m => m.NewEmail)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .NotEqual(m => m.User.Email).WithMessage("New email must be different from current email.")
            .MaximumLength(100);
    }
}