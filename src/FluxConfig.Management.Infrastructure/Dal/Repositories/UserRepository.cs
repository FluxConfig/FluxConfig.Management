using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Models.Enums;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<long>> AddUserCredentials(UserCredentialsEntity[] entities,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO user_credentials (username, email, password, role)
    SELECT username, email, password, role 
    FROM UNNEST(@Entities::user_credentials_type[])
    RETURNING id;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var sqlParameters = new
        {
            Entities = entities
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

            return ids.ToList();
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
SELECT uc.id, uc.username, uc.email, uc.password, uc.role
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

    public async Task<IReadOnlyList<UserCredentialsEntity>> GetAllUsers(CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_credentials;
";
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<UserCredentialsEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task UpdateUserEmail(long userId, string newEmail, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE user_credentials
    SET email = @NewEmail
    WHERE id = @UserId
    returning id;
";
        var sqlParameters = new
        {
            NewEmail = newEmail,
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        IEnumerable<long> editedIds;
        try
        {
            editedIds = await connection.QueryAsync<long>(
                new CommandDefinition(
                    commandText: sqlQuery,
                    parameters: sqlParameters,
                    cancellationToken: cancellationToken
                )
            );
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == "23505")
            {
                throw new EntityAlreadyExistsException("Entity already exists.");
            }

            throw;
        }

        if (!editedIds.Any())
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }
    }

    public async Task UpdateUserPassword(long userId, string newHashedPassword, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE user_credentials
    SET password = @NewPassword
    WHERE id = @UserId
    returning id;
";
        var sqlParameters = new
        {
            NewPassword = newHashedPassword,
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        IEnumerable<long> editedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!editedIds.Any())
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }
    }

    public async Task UpdateUserUsername(long userId, string newUsername, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE user_credentials
    SET username = @NewUsername
    WHERE id = @UserId
    returning id;
";
        var sqlParameters = new
        {
            NewUsername = newUsername,
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        IEnumerable<long> editedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!editedIds.Any())
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }
    }

    public async Task UpdateUserRole(long userId, UserGlobalRole newRole, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE user_credentials
    SET role = @NewRole::user_global_role_enum
    WHERE id = @UserId
    returning id;
";
        var sqlParameters = new
        {
            NewRole = newRole.ToString().ToLower(),
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        IEnumerable<long> editedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!editedIds.Any())
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }
    }

    public async Task DeleteUser(long userId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM user_credentials
    WHERE id = @UserId
    RETURNING id;
";

        var sqlParameters = new
        {
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var deletedId = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!deletedId.Any())
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }
    }
}