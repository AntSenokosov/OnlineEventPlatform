using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services.Interfaces;

public interface IDbContextWrapper<T>
    where T : DbContext
{
    public T DbContext { get; }
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}