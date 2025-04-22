using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class ConfigurationKeysRepository : BaseRepository, IConfigurationKeysRepository
{
    public ConfigurationKeysRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }

    public async Task AddKeys(ConfigurationKeyEntity[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO configuration_keys (id, configuration_id, role_permission, expiration_date)
    SELECT id, configuration_id, role_permission, expiration_date
    FROM UNNEST(@Entities::configuration_key_type[]);
";
        var sqlParameters = new
        {
            Entities = entities
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        try
        {
            await connection.QueryAsync<ConfigurationKeyEntity>(
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

            throw;
        }
    }

    public async Task DeleteKey(string id, long configurationId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM configuration_keys
    WHERE id = @Id AND configuration_id = @ConfigId
    RETURNING id;
";
        var sqlParameters = new
        {
            Id = id,
            ConfigId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var deletedIds = await connection.QueryAsync<string>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!deletedIds.Any())
        {
            throw new EntityNotFoundException("Key entity could not be found.");
        }
    }

    public async Task<IReadOnlyList<ConfigurationKeyEntity>> GetAllForConfiguration(
        long configurationId,
        DateTimeOffset currentUtcTime,
        CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configuration_keys
    WHERE configuration_id = @ConfigId
    AND expiration_date >= @CurrentUtcTime;
";
        var sqlParameters = new
        {
            CurrentUtcTime = currentUtcTime,
            ConfigId = configurationId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationKeyEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }
}