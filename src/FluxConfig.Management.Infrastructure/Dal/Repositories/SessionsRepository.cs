using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class SessionsRepository: BaseRepository, ISessionsRepository
{
    public SessionsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<UserSessionEntity>> GetUserSession(string sessionId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task CreateUserSession(UserSessionEntity entity, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    // TODO: Не должен бросать исключений 
    public async Task DeleteUserSession(string sessionId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}