using System.Transactions;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using Npgsql;

namespace FluxConfig.Management.Infrastructure.Dal.Repositories;

public abstract class BaseRepository: IDbRepository
{
    private readonly NpgsqlDataSource _dataSource;

    protected BaseRepository(NpgsqlDataSource npgsqlDataSource)
    {
        _dataSource = npgsqlDataSource;
    }

    protected async Task<NpgsqlConnection> GetAndOpenConnection(CancellationToken cancellationToken)
    {
        return await _dataSource.OpenConnectionAsync(cancellationToken);
    }
    
    public TransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            scopeOption: TransactionScopeOption.Required,
            transactionOptions: new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = TimeSpan.FromSeconds(5)
            },
            asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled
        );
    }
}