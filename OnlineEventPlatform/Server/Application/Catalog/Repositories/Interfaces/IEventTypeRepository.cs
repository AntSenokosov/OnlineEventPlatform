using Domain.Catalog.Entities;

namespace Application.Catalog.Repositories.Interfaces;

public interface IEventTypeRepository
{
    public Task<IEnumerable<EventType>> GetTypes();
}