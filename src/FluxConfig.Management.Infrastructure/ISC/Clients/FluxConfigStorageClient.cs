using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Infrastructure.ISC.Clients.Interfaces;
using FluxConfig.Management.Infrastructure.ISC.Extensions;
using FluxConfig.Management.Infrastructure.ISC.GrpcContracts.Storage;
using Grpc.Core;

namespace FluxConfig.Management.Infrastructure.ISC.Clients;

public class FluxConfigStorageClient : IFluxConfigStorageClient
{
    private readonly Storage.StorageClient _client;

    public FluxConfigStorageClient(Storage.StorageClient client)
    {
        _client = client;
    }

    public async Task CreateConfiguration(string key, string tag, CancellationToken cancellationToken)
    {
        try
        {
            await CreateConfigurationUnsafe(key, tag, cancellationToken);
        }
        catch (RpcException ex)
        {
            InfrastructureException iscException = ex.MapExceptionToIscException();
            throw iscException;
        }
    }

    private async Task CreateConfigurationUnsafe(string key, string tag, CancellationToken cancellationToken)
    {
        await _client.CreateServiceConfigurationAsync(
            request: new CreateServiceConfigRequest
            {
                ConfigurationKey = key,
                ConfigurationTag = tag
            },
            cancellationToken: cancellationToken
        );
    }

    public async Task DeleteConfiguration(string key, List<string> tags, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteConfigurationUnsafe(key, tags, cancellationToken);
        }
        catch (RpcException ex)
        {
            InfrastructureException iscException = ex.MapExceptionToIscException();
            throw iscException;
        }
    }

    private async Task DeleteConfigurationUnsafe(string key, List<string> tags, CancellationToken cancellationToken)
    {
        DeleteServiceConfigRequest request = new DeleteServiceConfigRequest
        {
            ConfigurationKey = key
        };
        request.ConfigurationTags.AddRange(tags);
        
        await _client.DeleteServiceConfigurationAsync(
            request: request,
            cancellationToken: cancellationToken
        );
    }
}