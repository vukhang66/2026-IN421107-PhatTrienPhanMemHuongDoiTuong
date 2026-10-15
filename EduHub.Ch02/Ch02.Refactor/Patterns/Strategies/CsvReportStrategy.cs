using System.Text;

namespace EduHub.Ch02.Patterns.Strategies;

public class CsvReportStrategy : IReportStrategy
{
    public string Name => "CSV";

    public string Render(IReadOnlyList<ClubStat> stats)
    {
        var builder = new StringBuilder("Club,Members,Events,Registered,CheckedIn");
        foreach (var stat in stats)
        {
            builder.Append(Environment.NewLine)
                .Append('"')
                .Append(stat.ClubName.Replace("\"", "\"\""))
                .Append("\",")
                .Append(stat.Members)
                .Append(',')
                .Append(stat.Events)
                .Append(',')
                .Append(stat.Registered)
                .Append(',')
                .Append(stat.CheckedIn);
        }

        return builder.ToString();
    }
}
