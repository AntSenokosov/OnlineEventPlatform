using Application.Catalog.Repositories.Interfaces;
using Application.Catalog.Services.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class CatalogService : BaseDataService<OnlineEventContext>, ICatalogService
{
    private readonly ICatalogRepository _catalogRepository;
    public CatalogService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        ICatalogRepository catalogRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task<IEnumerable<OnlineEventDto>> GetCatalog()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var items = await _catalogRepository.GetCatalog();

            var itemsDto = items.Select(e => new OnlineEventDto()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Time = e.Time,
                AboutEvent = e.AboutEvent
            });

            return itemsDto;
        });
    }

    public async Task<EventItem> GetItem(int id)
    {
        return await ExecuteSafeAsync(async () => await _catalogRepository.GetItem(id));
    }
}