using System.Transactions;

namespace FluxConfig.Management.Domain.Contracts.Dal.Interfaces;

public interface IDbRepository
{
    public TransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}