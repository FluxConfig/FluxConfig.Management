using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IUserService
{
    public Task ChangeUserEmail(ChangeUserEmailModel changeEmailModel, CancellationToken cancellationToken);
    public Task ChangeUserPassword(ChangeUserPasswordModel changePasswordModel, CancellationToken cancellationToken);
    public Task ChangeUserUsername(ChangeUserUsernameModel changeUsernameModel, CancellationToken cancellationToken);
    public Task ChangeUserRole(UserGlobalRole newRole, long userId, long adminId, CancellationToken cancellationToken);
    public Task DeleteUser(long userId, CancellationToken cancellationToken);
    public Task<IReadOnlyList<UserModel>> GetAllUsers(CancellationToken cancellationToken);
    public Task<UserModel> GetUser(long userId, CancellationToken cancellationToken);
}