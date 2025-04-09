using FluxConfig.Management.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface ISessionsRepository: IDbRepository
{
    public Task<IReadOnlyList<UserSessionEntity>> GetUserSession(string sessionId, CancellationToken cancellationToken);
    public Task CreateUserSession(UserSessionEntity entity, CancellationToken cancellationToken);
    public Task DeleteUserSession(string sessionId, CancellationToken cancellationToken);
}