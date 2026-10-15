namespace EduHub.Ch02.Patterns.Strategies;

public interface IReportStrategy
{
    string Name { get; }
    string Render(IReadOnlyList<ClubStat> stats);
}
