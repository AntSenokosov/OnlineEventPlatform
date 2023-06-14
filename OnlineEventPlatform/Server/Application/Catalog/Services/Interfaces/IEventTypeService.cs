using Domain.Catalog.Entities;

namespace Application.Catalog.Services.Interfaces;

public interface IEventTypeService
{
    public Task<IEnumerable<EventTypeDto>> GetTypes();
}