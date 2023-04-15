using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Database;

public partial class OnlineEventContext : DbContext
{
    private IDbContextTransaction? _currentTransaction;

    public OnlineEventContext(DbContextOptions<OnlineEventContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public void BeginTransaction()
    {
        if (_currentTransaction != null)
        {
            return;
        }

        if (!Database.IsInMemory())
        {
            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }
    }

    public void CommitTransaction()
    {
        try
        {
            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}