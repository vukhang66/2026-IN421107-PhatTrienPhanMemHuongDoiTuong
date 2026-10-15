using EduHub.Ch02.Domain;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

public class EventService : IEventService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly IEventRepository _events;
    private readonly List<IEventObserver> _observers = new();

    public EventService(IClubRepository clubs, IMemberRepository members, IEventRepository events)
    {
        _clubs = clubs;
        _members = members;
        _events = events;
    }

    public void Subscribe(IEventObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IEventObserver observer) => _observers.Remove(observer);

    public Event CreateEvent(Guid clubId, string title, DateTime date)
    {
        var club = _clubs.GetById(clubId)
            ?? throw new InvalidOperationException("Club not found");
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Event title is required", nameof(title));

        var ev = new Event { Id = Guid.NewGuid(), ClubId = clubId, Title = title, Date = date };
        _events.Add(ev);

        foreach (var observer in _observers.ToArray())
            observer.OnEventCreated(ev, club);

        return ev;
    }

    public void Register(Guid eventId, Guid memberId)
    {
        var ev = GetEvent(eventId);
        var member = _members.GetById(memberId)
            ?? throw new InvalidOperationException("Member not found");
        if (member.ClubId != ev.ClubId)
            throw new InvalidOperationException("Member does not belong to this club");

        ev.Register(memberId);
    }

    public void CheckIn(Guid eventId, Guid memberId) => GetEvent(eventId).CheckIn(memberId);

    private Event GetEvent(Guid id) =>
        _events.GetById(id) ?? throw new InvalidOperationException("Event not found");
}
