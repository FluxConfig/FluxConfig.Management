using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IUserRepository: IDbRepository
{
    public Task<IReadOnlyList<long>> AddUserCredentials(UserCredentialsEntity[] entities,
        CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserByEmail(string userEmail, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserBySessionId(string sessionId, DateTimeOffset curTime, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserById(long userId, CancellationToken cancellationToken);
    public Task<IReadOnlyList<UserCredentialsEntity>> GetAllUsers(CancellationToken cancellationToken);
    public Task UpdateUserEmail(long userId, string newEmail, CancellationToken cancellationToken);
    public Task UpdateUserPassword(long userId, string newHashedPassword, CancellationToken cancellationToken);
    public Task UpdateUserUsername(long userId, string newUsername, CancellationToken cancellationToken);
    public Task UpdateUserRole(long userId, UserGlobalRole newRole, CancellationToken cancellationToken);
    public Task DeleteUser(long userId, CancellationToken cancellationToken);
}
