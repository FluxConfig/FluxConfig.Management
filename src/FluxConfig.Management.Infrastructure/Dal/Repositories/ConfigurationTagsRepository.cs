using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
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
            throw new EntityNotFoundException("Tag entity could not be found.");
        }

        return entity;
    }
}