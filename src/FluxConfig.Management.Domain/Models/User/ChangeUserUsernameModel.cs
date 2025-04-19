namespace FluxConfig.Management.Domain.Models.User;

public record ChangeUserUsernameModel(
    UserModel User,
    string NewUsername 
);