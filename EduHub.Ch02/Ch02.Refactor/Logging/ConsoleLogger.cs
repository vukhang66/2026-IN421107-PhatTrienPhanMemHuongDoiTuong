namespace EduHub.Ch02.Logging;

public class ConsoleLogger : IAppLogger
{
    public void Info(string message) => Console.WriteLine($"  [LOG] {message}");
}
