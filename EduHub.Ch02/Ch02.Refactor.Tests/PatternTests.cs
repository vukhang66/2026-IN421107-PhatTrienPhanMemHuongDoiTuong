using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Services;

namespace Ch02.Refactor.Tests;

public class PatternTests
{
    private sealed class FakeLogger : IAppLogger
    {
        public List<string> Messages { get; } = new();
        public void Info(string message) => Messages.Add(message);
    }

    private sealed class FakeNotificationService : INotificationService
    {
        public List<(string To, string Message)> Messages { get; } = new();

        public void Send(string to, string message) => Messages.Add((to, message));
    }

    private sealed class SpyObserver : IEventObserver
    {
        public int Calls { get; private set; }

        public void OnEventCreated(Event ev, Club club) => Calls++;
    }

    [Theory]
    [InlineData(NotificationChannel.Email, typeof(EmailNotificationService))]
    [InlineData(NotificationChannel.Sms, typeof(SmsNotificationService))]
    [InlineData(NotificationChannel.Slack, typeof(SlackNotificationService))]
    public void Factory_Create_ReturnsServiceForRequestedChannel(
        NotificationChannel channel, Type expectedType)
    {
        // Arrange
        INotificationServiceFactory factory = new NotificationServiceFactory();

        // Act
        var service = factory.Create(channel);

        // Assert
        Assert.IsType(expectedType, service);
    }

    [Fact]
    public void Factory_Create_UnknownChannel_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        INotificationServiceFactory factory = new NotificationServiceFactory();

        // Act
        var act = () => factory.Create((NotificationChannel)99);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void Decorator_Add_LogsAndStoresClubInInnerRepository()
    {
        // Arrange
        var logger = new FakeLogger();
        var inner = new InMemoryClubRepository();
        IClubRepository repository = new LoggingClubRepository(inner, logger);
        var club = Club.Create("Tech Club", "mô tả");

        // Act
        repository.Add(club);

        // Assert
        Assert.Contains(logger.Messages, message => message.Contains("Tech Club"));
        Assert.Same(club, inner.GetById(club.Id));
    }

    [Fact]
    public void Strategy_Render_SameStatisticsAsConsoleAndCsv()
    {
        // Arrange
        var stats = new List<ClubStat> { new("Tech Club", 2, 1, 2, 1) };

        // Act
        var console = new ConsoleReportStrategy().Render(stats);
        var csv = new CsvReportStrategy().Render(stats);

        // Assert
        Assert.StartsWith("Club: Tech Club", console);
        Assert.StartsWith("Club,Members,Events,Registered,CheckedIn", csv);
        Assert.Contains("\"Tech Club\",2,1,2,1", csv);
    }

    [Fact]
    public void Strategy_Csv_EscapesQuotesInClubName()
    {
        // Arrange
        var stats = new List<ClubStat> { new("Tech \"Club\"", 1, 0, 0, 0) };

        // Act
        var csv = new CsvReportStrategy().Render(stats);

        // Assert
        Assert.Contains("\"Tech \"\"Club\"\"\",1,0,0,0", csv);
    }

    [Fact]
    public void Observer_CreateEvent_NotifiesEverySubscriber()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        var service = new EventService(clubs, new InMemoryMemberRepository(), new InMemoryEventRepository());
        var first = new SpyObserver();
        var second = new SpyObserver();
        service.Subscribe(first);
        service.Subscribe(second);

        // Act
        service.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));

        // Assert
        Assert.Equal(1, first.Calls);
        Assert.Equal(1, second.Calls);
    }

    [Fact]
    public void Observer_Unsubscribe_StopsNotifications()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        var service = new EventService(clubs, new InMemoryMemberRepository(), new InMemoryEventRepository());
        var observer = new SpyObserver();
        service.Subscribe(observer);
        service.Unsubscribe(observer);

        // Act
        service.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));

        // Assert
        Assert.Equal(0, observer.Calls);
    }

    [Fact]
    public void EmailNotifier_EventCreated_SendsMessageToEveryClubMember()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var club = new Club
        {
            Id = clubId,
            Name = "Tech Club",
            Description = "mô tả"
        };
        var clubMembers = new InMemoryMemberRepository();
        var first = new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = "Nguyễn Văn A",
            Email = "a@example.com"
        };
        var second = new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = "Trần Thị B",
            Email = "b@example.com"
        };
        clubMembers.Add(first);
        clubMembers.Add(second);
        var notifier = new FakeNotificationService();
        var observer = new EmailNotifier(clubMembers, notifier);
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = "Workshop SOLID",
            Date = new DateTime(2026, 11, 20)
        };

        // Act
        observer.OnEventCreated(ev, club);

        // Assert
        Assert.Equal(new[] { "a@example.com", "b@example.com" }, notifier.Messages.Select(m => m.To));
        Assert.All(notifier.Messages, message => Assert.Contains("Workshop SOLID", message.Message));
    }

    [Fact]
    public void ClubService_AddMember_WhenClubIsFull_ThrowsAndDoesNotAddMember()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var members = new InMemoryMemberRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        for (var i = 0; i < Club.MaxMembers; i++)
        {
            members.Add(new Member
            {
                Id = Guid.NewGuid(),
                ClubId = club.Id,
                Name = $"Member {i}",
                Email = $"member{i}@example.com"
            });
        }

        var service = new ClubService(clubs, members, new FakeNotificationService(), new FakeLogger());

        // Act
        var act = () => service.AddMember(club.Id, "Extra member", "extra@example.com");

        // Assert
        Assert.Throws<InvalidOperationException>(act);
        Assert.Equal(Club.MaxMembers, members.GetByClub(club.Id).Count);
    }

    [Fact]
    public void ReportService_Generate_ReportsMembersAndEventAttendance()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var members = new InMemoryMemberRepository();
        var events = new InMemoryEventRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = club.Id,
            Name = "Nguyễn Văn A",
            Email = "a@example.com"
        });
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = club.Id,
            Title = "Workshop SOLID",
            Date = new DateTime(2026, 11, 20)
        };
        ev.Register(Guid.NewGuid());
        ev.Register(Guid.NewGuid());
        ev.CheckIn(ev.RegisteredMemberIds.First());
        events.Add(ev);
        var service = new ReportService(clubs, members, events);

        // Act
        var report = service.Generate(new CsvReportStrategy());

        // Assert
        Assert.Contains("\"Tech Club\",1,1,2,1", report);
    }
}
