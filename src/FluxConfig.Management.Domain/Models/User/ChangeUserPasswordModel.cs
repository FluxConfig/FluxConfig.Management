namespace FluxConfig.Management.Domain.Models.User;

public record ChangeUserPasswordModel(
    UserModel User,
    string NewPassword
);