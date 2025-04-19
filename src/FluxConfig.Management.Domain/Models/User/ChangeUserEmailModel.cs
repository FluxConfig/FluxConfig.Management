namespace FluxConfig.Management.Domain.Models.User;

public record ChangeUserEmailModel(
    UserModel User,
    string  NewEmail
);