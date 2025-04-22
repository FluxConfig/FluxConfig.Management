using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Models.Configuration;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class UserConfigurationRepository : BaseRepository, IUserConfigurationRepository
{
    public UserConfigurationRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<UserConfigurationEntity> GetUserConfiguration(long userId, long configurationId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_configurations
    WHERE user_id = @UserId AND configuration_id = @ConfigurationId;
";

        var sqlParameters = new
        {
            UserId = userId,
            ConfigurationId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<UserConfigurationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        List<UserConfigurationEntity> entitiesList = entities.ToList();

        if (entitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Entity could not be found.");
        }

        return entitiesList[0];
    }

    public async Task AddUsersToConfiguration(UserConfigurationEntity[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO user_configurations (user_id, role, configuration_id)
    SELECT user_id, role, configuration_id
    FROM UNNEST(@Entities::user_configurations_type[]);
";
        var sqlParameters = new
        {
            Entities = entities
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        try
        {
            await connection.QueryAsync<UserConfigurationEntity>(
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
                throw new EntityNotFoundException("Configuration id Foreign key not found.");
            }

            if (ex.SqlState != "23505")
            {
                throw;
            }
        }
    }

    public async Task UpdateUserConfigRole(UserConfigurationEntity entity, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE user_configurations
    SET role = @NewRole::user_config_role_enum 
    WHERE user_id = @UserId
    AND configuration_id = @ConfigurationId
    returning user_id;
";
        var sqlParameters = new
        {
            NewRole = entity.Role.ToString().ToLower(),
            UserId = entity.UserId,
            ConfigurationId = entity.ConfigurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var updatedUserIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!updatedUserIds.Any())
        {
            throw new EntityNotFoundException("User doesnt belong to configuration.");
        }
    }

    public async Task DeleteUserFromConfiguration(long userId, long configurationId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM user_configurations
    WHERE user_id = @UserId
    AND configuration_id = @ConfigurationId;
";
        var sqlParameters = new
        {
            UserId = userId,
            ConfigurationId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        await connection.QueryAsync<UserConfigurationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<IReadOnlyList<ConfigurationUserViewEntity>> GetConfigurationUsers(long configurationId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configuration_users_view
    WHERE configuration_id = @ConfigurationId;
";
        var sqlParameters = new
        {
            ConfigurationId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationUserViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }
}