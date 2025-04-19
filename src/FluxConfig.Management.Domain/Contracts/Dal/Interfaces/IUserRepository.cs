using FluxConfig.Management.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IUserRepository: IDbRepository
{
    public Task<IReadOnlyList<long>> AddUserCredentials(UserCredentialsEntity[] entities,
        CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserByEmail(string userEmail, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserBySessionId(string sessionId, DateTimeOffset curTime, CancellationToken cancellationToken);
    public Task<UserCredentialsEntity> GetUserById(long userId, CancellationToken cancellationToken);
}
