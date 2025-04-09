using FluxConfig.Management.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IUserRepository: IDbRepository
{
    public Task<long> AddUserCredentials(UserCredentialsEntity entity, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserByEmail(string userEmail, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserBySessionId(string sessionId, DateTimeOffset curTime, CancellationToken cancellationToken);
    public Task<long> AddUserGlobalRole(UserGlobalRoleEntity entity, CancellationToken cancellationToken);
    public Task<IReadOnlyList<UserGlobalRoleEntity>> GetUserGlobalRoles(long userId, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserById(long userId, CancellationToken cancellationToken);
}
