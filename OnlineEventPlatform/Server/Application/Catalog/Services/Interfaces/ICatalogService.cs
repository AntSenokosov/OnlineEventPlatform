namespace Application.Catalog.Services.Interfaces;

public interface ICatalogService
{
    public Task<IEnumerable<OnlineEventDto>> GetCatalog();
    public Task<EventItem> GetItem(int id);
}