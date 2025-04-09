using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<long> AddUserCredentials(UserCredentialsEntity entity, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO user_credentials (username, email, password)
    VALUES (@Username, @Email, @Password)
    RETURNING id;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            entity.Username,
            entity.Email,
            entity.Password
        };

        try
        {
            var ids = await connection.QueryAsync<long>(
                new CommandDefinition(
                    commandText: sqlQuery,
                    parameters: sqlParameters,
                    cancellationToken: cancellationToken
                )
            );

            return ids.ToList()[0];
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == "23505")
            {
                throw new EntityAlreadyExistsException("Entity already exists.");
            }

            throw;
        }
    }

    public async Task<UserCredentialsEntity> GetUserByEmail(string userEmail, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_credentials WHERE email = @Email;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Email = userEmail
        };

        var userEntity = await connection.QueryAsync<UserCredentialsEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entityList = userEntity.ToList();
        if (entityList.Count == 0)
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }

        return entityList[0];
    }
    
    public async Task<UserCredentialsEntity> GetUserBySessionId(string sessionId, DateTimeOffset curTime,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT uc.id, uc.username, uc.email, uc.password 
FROM user_sessions
    INNER JOIN user_credentials as uc
    ON user_sessions.user_id = uc.id
    WHERE user_sessions.id = @SessionId
    AND user_sessions.expiration_date >= @CurrentTime;
";
        var sqlParameters = new
        {
            SessionId = sessionId,
            CurrentTime = curTime
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var userEntities = await connection.QueryAsync<UserCredentialsEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );
        
        var entityList = userEntities.ToList();
        if (entityList.Count == 0)
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }

        return entityList[0];
    }

    public async Task<UserCredentialsEntity> GetUserById(long userId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_credentials
    WHERE id = @UserId;
";

        var sqlParameters = new
        {
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var userEntities = await connection.QueryAsync<UserCredentialsEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        List<UserCredentialsEntity> entitiesList = userEntities.ToList();

        if (entitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }

        return entitiesList[0];
    }

    public async Task<IReadOnlyList<long>> AddUserGlobalRoles(UserGlobalRoleEntity[] entities,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO user_global_roles (user_id, role)
    SELECT user_id, role
    FROM UNNEST(@Roles::user_global_role_type[])
    RETURNING id;
";

        var sqlParameters = new
        {
            Roles = entities
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        IEnumerable<long> createdIds;

        try
        {
            createdIds = await connection.QueryAsync<long>(
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

        return createdIds.ToList();
    }

    public async Task<IReadOnlyList<UserGlobalRoleEntity>> GetUserGlobalRoles(long userId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_global_roles
    WHERE user_id = @UserId;
";

        var sqlParameter = new
        {
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<UserGlobalRoleEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameter,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }
}