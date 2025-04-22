using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class ConfigurationTagsRepository: BaseRepository, IConfigurationTagsRepository
{
    public ConfigurationTagsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }
}