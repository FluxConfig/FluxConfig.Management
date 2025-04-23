using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class ConfigurationsRepository : BaseRepository, IConfigurationsRepository
{
    public ConfigurationsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task<IReadOnlyList<UserConfigurationsViewEntity>> GetUserConfigurations(long userId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_configurations_view
    WHERE user_id = @UserId;
";

        var sqlParameters = new
        {
            UserId = userId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<UserConfigurationsViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task<IReadOnlyList<ConfigurationEntity>> GetAllConfigurations(CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configurations;
";

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task ChangeConfigurationName(long id, string newName, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE configurations
    SET name = @Name
    WHERE id = @ConfigurationId
    RETURNING id;
";

        var sqlParameters = new
        {
            Name = newName,
            ConfigurationId = id
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var updatedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!updatedIds.Any())
        {
            throw new EntityNotFoundException("Configuration entity could not be found.");
        }
    }

    public async Task ChangeConfigurationDescription(long id, string newDescription,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE configurations
    SET description = @Description
    WHERE id = @ConfigurationId
    RETURNING id;
";

        var sqlParameters = new
        {
            Description = newDescription,
            ConfigurationId = id
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var updatedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!updatedIds.Any())
        {
            throw new EntityNotFoundException("Configuration entity could not be found.");
        }
    }

    public async Task<UserConfigurationsViewEntity> GetUserConfigurationById(long userId, long configurationId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM user_configurations_view
    WHERE user_id = @UserId
    AND configuration_id = @ConfigurationId;
";

        var sqlParameters = new
        {
            UserId = userId,
            ConfigurationId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<UserConfigurationsViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entitiesList = entities.ToList();

        if (entitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Configuration entity could not be found.");
        }

        return entitiesList[0];
    }

    public async Task<ConfigurationEntity> GetConfigurationById(long configurationId,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configurations
    WHERE id = @ConfigurationId;
";

        var sqlParameters = new
        {
            ConfigurationId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entitiesList = entities.ToList();

        if (entitiesList.Count == 0)
        {
            throw new EntityNotFoundException("Configuration entity could not be found.");
        }

        return entitiesList[0];
    }

    public async Task<long> Create(ConfigurationEntity entity, CancellationToken cancellationToken)
    {
        const string slqQuery = @"
INSERT INTO configurations (name, storage_key, description)
    VALUES (@Name, @StorageKey, @Description)
    RETURNING id;
";

        var sqlParameters = new
        {
            entity.Name,
            entity.StorageKey,
            entity.Description
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var createdIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: slqQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var createdIdList = createdIds.ToList();

        if (createdIdList.Count == 0)
        {
            throw new EntityNotFoundException("Configuration could not be created.");
        }

        return createdIdList[0];
    }

    public async Task Delete(long configurationId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM configurations
    WHERE id = @ConfigurationId
    RETURNING id;
";
        var sqlParameters = new
        {
            ConfigurationId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var deletedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!deletedIds.Any())
        {
            throw new EntityNotFoundException("Configuration entity could not be found.");
        }
    }
}