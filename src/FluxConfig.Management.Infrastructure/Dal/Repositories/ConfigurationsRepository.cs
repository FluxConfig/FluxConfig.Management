using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public class ConfigurationsRepository: BaseRepository, IConfigurationsRepository
{
    public ConfigurationsRepository(NpgsqlDataSource npgsqlDataSource) : base(npgsqlDataSource)
    {
    }
}