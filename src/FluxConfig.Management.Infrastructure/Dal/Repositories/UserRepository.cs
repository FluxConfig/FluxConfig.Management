using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class UserRepository: BaseRepository, IUserRepository
{
    public UserRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<long> AddUserCredentials(UserCredentialsEntity entity, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<UserCredentialsEntity> GetUserByEmail(string userEmail, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    //TODO: Сделать выборку по времени
    public async Task<UserCredentialsEntity> GetUserBySessionId(string sessionId, DateTimeOffset curTime, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<UserCredentialsEntity> GetUserById(long userId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
    
    public async Task<long> AddUserGlobalRole(UserGlobalRoleEntity entity, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    // TODO: Не должен бросать исключений
    public async Task<IReadOnlyList<UserGlobalRoleEntity>> GetUserGlobalRoles(long userId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}