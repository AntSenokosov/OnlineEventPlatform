using Domain.Catalog.Entities;

namespace Application.Catalog.Repositories.Interfaces;

public interface ICatalogRepository
{
    public Task<IEnumerable<OnlineEvent>> GetCatalog();
}