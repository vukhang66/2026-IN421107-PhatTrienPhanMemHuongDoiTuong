using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public interface IEventRepository
{
    IReadOnlyList<Event> GetByClub(Guid clubId);
    Event? GetById(Guid id);
    void Add(Event ev);
}
