using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

public class ReportService : IReportService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly IEventRepository _events;

    public ReportService(IClubRepository clubs, IMemberRepository members, IEventRepository events)
    {
        _clubs = clubs;
        _members = members;
        _events = events;
    }

    public string Generate(IReportStrategy strategy)
    {
        var stats = _clubs.GetAll().Select(club =>
        {
            var events = _events.GetByClub(club.Id);
            return new ClubStat(
                club.Name,
                _members.GetByClub(club.Id).Count,
                events.Count,
                events.Sum(ev => ev.RegisteredMemberIds.Count),
                events.Sum(ev => ev.CheckedInMemberIds.Count));
        }).ToList();

        return strategy.Render(stats);
    }
}
