namespace FluxConfig.Management.Infrastructure.ISC.Clients.Interfaces;

public interface IFluxConfigStorageClient
{
    public Task CreateConfiguration(string key, string tag, CancellationToken cancellationToken);

    public Task DeleteConfiguration(string key, List<string> tags, CancellationToken cancellationToken);
}