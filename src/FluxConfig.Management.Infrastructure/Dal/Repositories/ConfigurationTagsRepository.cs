using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Models.Enums;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class ConfigurationTagsRepository: BaseRepository, IConfigurationTagsRepository
{
    public ConfigurationTagsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }


    public async Task<ConfigurationTagsViewEntity> GetTagWithConfigurationByConfigId(long configurationId, string tag, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configuration_tags_view
    WHERE configuration_id = @ConfigurationId
    AND tag_name = @TagName;
";
        var sqlParameters = new
        {
            ConfigurationId = configurationId,
            TagName = tag
        };
        
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationTagsViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entity = entities.FirstOrDefault();

        if (entity == null)
        {
            throw new EntityNotFoundException("Configuration tag entity could not be found.");
        }

        return entity;
    }

    public async Task<ConfigurationTagsViewEntity> GetTagWithConfigurationByTagId(long tagId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configuration_tags_view
    WHERE tag_id = @TagId;
";
        var sqlParameters = new
        {
            TagId = tagId
        };
        
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationTagsViewEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        var entity = entities.FirstOrDefault();

        if (entity == null)
        {
            throw new EntityNotFoundException("Configuration tag entity could not be found.");
        }

        return entity;
    }

    public async Task<IReadOnlyList<ConfigurationTagEntity>> GetConfigurationTags(long configurationId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
SELECT * FROM configuration_tags
    WHERE configuration_id = @ConfigId;
";
        var sqlParameters = new
        {
            ConfigId = configurationId
        };
        
        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var entities = await connection.QueryAsync<ConfigurationTagEntity>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        return entities.ToList();
    }

    public async Task UpdateTagDescription(long tagId, string newDescription, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE configuration_tags
    SET description = @NewDescription
    WHERE id = @TagId
    RETURNING id;
";

        var sqlParameters = new
        {
            NewDescription = newDescription,
            TagId = tagId
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
            throw new EntityNotFoundException("Configuration tag entity could not be found.");
        }
    }

    public async Task UpdateTagRequiredRole(long tagId, UserConfigRole newRole, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
UPDATE configuration_tags
    SET required_role = @NewRole::user_config_role_enum
    WHERE id = @TagId
    RETURNING id;
";
        var sqlParameters = new
        {
            NewRole = newRole.ToString().ToLower(),
            TagId = tagId
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        var editedIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                commandText: sqlQuery,
                parameters: sqlParameters,
                cancellationToken: cancellationToken
            )
        );

        if (!editedIds.Any())
        {
            throw new EntityNotFoundException("Configuration tag entity could not be found.");
        }
    }

    public async Task CreateConfigurationTags(ConfigurationTagEntity[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
INSERT INTO configuration_tags (configuration_id, tag, description, required_role) 
    SELECT configuration_id, tag, description, required_role
    FROM UNNEST(@Entities::configuration_tag_type[]);
";

        var sqlParameters = new
        {
            Entities = entities
        };

        await using NpgsqlConnection connection = await GetAndOpenConnection(cancellationToken);

        try
        {
            await connection.QueryAsync<ConfigurationTagEntity>(
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
                throw new EntityNotFoundException("Configuration Foreign key not found.");
            }
            if (ex.SqlState == "23505")
            {
                throw new EntityAlreadyExistsException("Entity already exists.");
            }

            throw;
        }
    }

    public async Task DeleteConfigurationTag(long tagId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
DELETE FROM configuration_tags
    WHERE id = @TagId
    RETURNING id;
";
        var sqlParameters = new
        {
            TagId = tagId
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
            throw new EntityNotFoundException("Configuration Tag entity could not be found.");
        }
    }
}