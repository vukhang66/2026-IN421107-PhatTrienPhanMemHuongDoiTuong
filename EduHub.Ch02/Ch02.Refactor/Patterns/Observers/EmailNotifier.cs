using EduHub.Ch02.Domain;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Patterns.Observers;

public class EmailNotifier : IEventObserver
{
    private readonly IMemberRepository _members;
    private readonly INotificationService _notifier;

    public EmailNotifier(IMemberRepository members, INotificationService notifier)
    {
        _members = members;
        _notifier = notifier;
    }

    public void OnEventCreated(Event ev, Club club)
    {
        foreach (var member in _members.GetByClub(club.Id))
            _notifier.Send(member.Email, $"New event '{ev.Title}' at {club.Name} on {ev.Date:dd/MM/yyyy}");
    }
}
