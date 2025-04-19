using FluentValidation;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Validators.User;

public class ChangeUserUsernameModelValidator : AbstractValidator<ChangeUserUsernameModel>
{
    public ChangeUserUsernameModelValidator()
    {
        RuleFor(m => m.NewUsername)
            .NotNull()
            .NotEmpty()
            .NotEqual(m => m.User.Username).WithMessage("New username must be different from current username.")
            .MinimumLength(6)
            .MaximumLength(50);
    }
}