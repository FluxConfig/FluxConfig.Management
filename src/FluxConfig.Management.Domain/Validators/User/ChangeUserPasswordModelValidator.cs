using FluentValidation;
using FluxConfig.Management.Domain.Hasher;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Validators.User;

public class ChangeUserPasswordModelValidator : AbstractValidator<ChangeUserPasswordModel>
{
    public ChangeUserPasswordModelValidator()
    {
        RuleFor(m => m.NewPassword)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50);

        RuleFor(m => m)
            .Must(m => !PasswordHasher.Verify(
                password: m.NewPassword,
                hashedPassword: m.User.Password
            ))
            .WithMessage("New password must be different from current password.");
    }
}