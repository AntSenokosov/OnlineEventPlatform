namespace Application.Catalog.Services.Interfaces;

public interface ICatalogService
{
    public Task<IEnumerable<OnlineEventDto>> GetCatalog();
}