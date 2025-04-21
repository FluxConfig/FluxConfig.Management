using Dapper;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
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
}