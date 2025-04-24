using FluentValidation;
using FluxConfig.Management.Domain.Models.Auth;

namespace FluxConfig.Management.Domain.Validators.Auth;

public class UserRegisterModelValidator: AbstractValidator<UserRegisterModel>
{
    public UserRegisterModelValidator()
    {
        RuleFor(m => m.Username)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50);
        
        RuleFor(m => m.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);
        
        RuleFor(m => m.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50);
    }
}