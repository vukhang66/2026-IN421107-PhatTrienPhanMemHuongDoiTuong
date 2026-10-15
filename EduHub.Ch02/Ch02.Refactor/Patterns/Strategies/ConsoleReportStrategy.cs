using System.Text;

namespace EduHub.Ch02.Patterns.Strategies;

public class ConsoleReportStrategy : IReportStrategy
{
    public string Name => "Console";

    public string Render(IReadOnlyList<ClubStat> stats)
    {
        var builder = new StringBuilder();
        foreach (var stat in stats)
        {
            builder.AppendLine(
                $"Club: {stat.ClubName} | Members: {stat.Members} | Events: {stat.Events} | " +
                $"Registered: {stat.Registered} | Checked-in: {stat.CheckedIn}");
        }

        return builder.ToString();
    }
}
