using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class SessionsRepository : BaseRepository, ISessionsRepository
{
    public SessionsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<UserSessionEntity>> GetUserSession(string sessionId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_sessions
    WHERE id = @SessionId;
";

        var sqlParameters = new
        {
            SessionId = sessionId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sessionEntities = await connection.QueryAsync<UserSessionEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return sessionEntities.ToList();
    }

    public async Task CreateUserSession(UserSessionEntity entity, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO user_sessions (id, user_id, expiration_date)
    VALUES (@Id, @UserId, @ExpirationDate);
";

        var sqlParameters = entity;

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        try
        {
            await connection.QueryAsync<UserSessionEntity>(
                new CommandDefinition(
                    commandText: sqlQuery,
                    parameters: sqlParameters,
                    cancellationToken: cancellationToken
                )
            );
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == "23503")
            {
                throw new EntityNotFoundException("UserId Foreign key not found.");
            }

            throw;
        }
    }

    public async Task DeleteUserSession(string sessionId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM user_sessions WHERE id = @SessionId;
";

        var sqlParameters = new
        {
            SessionId = sessionId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);
        
        await connection.QueryAsync<UserSessionEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );
    }
}