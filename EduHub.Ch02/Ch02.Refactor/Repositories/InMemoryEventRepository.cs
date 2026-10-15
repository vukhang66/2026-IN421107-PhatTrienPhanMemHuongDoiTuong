using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _store = new();

    public IReadOnlyList<Event> GetByClub(Guid clubId) =>
        _store.Where(ev => ev.ClubId == clubId).ToList();

    public Event? GetById(Guid id) => _store.FirstOrDefault(ev => ev.Id == id);
    public void Add(Event ev) => _store.Add(ev);
}
