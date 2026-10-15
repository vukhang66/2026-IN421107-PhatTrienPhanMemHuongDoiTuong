using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Services;

Console.OutputEncoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

IAppLogger logger = new ConsoleLogger();

INotificationServiceFactory notificationFactory = new NotificationServiceFactory();
INotificationService notifier = notificationFactory.Create(NotificationChannel.Email);

IClubRepository clubRepository = new LoggingClubRepository(new InMemoryClubRepository(), logger);
IMemberRepository memberRepository = new InMemoryMemberRepository();
IEventRepository eventRepository = new InMemoryEventRepository();

IClubService clubService = new ClubService(clubRepository, memberRepository, notifier, logger);
IEventService eventService = new EventService(clubRepository, memberRepository, eventRepository);
IReportService reportService = new ReportService(clubRepository, memberRepository, eventRepository);

Console.WriteLine("== 1. Tạo CLB và thêm thành viên ==");
var club = clubService.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@gmail.com");
var firstMember = clubService.AddMember(club.Id, "Nguyễn Văn A", "a@student.dthu.edu.vn");
var secondMember = clubService.AddMember(club.Id, "Trần Thị B", "b@student.dthu.edu.vn");

Console.WriteLine("\n== 2. Observer: sự kiện thông báo cho các observer ==");
eventService.Subscribe(new LogNotifier(logger));
eventService.Subscribe(new EmailNotifier(memberRepository, notifier));
var ev = eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));
eventService.Register(ev.Id, firstMember.Id);
eventService.Register(ev.Id, secondMember.Id);
eventService.CheckIn(ev.Id, firstMember.Id);

Console.WriteLine("\n== 3. Strategy: báo cáo Console ==");
Console.Write(reportService.Generate(new ConsoleReportStrategy()));

Console.WriteLine("== 3. Strategy: báo cáo CSV ==");
Console.WriteLine(reportService.Generate(new CsvReportStrategy()));

Console.WriteLine("== 4. Factory: tạo dịch vụ theo kênh ==");
foreach (var channel in Enum.GetValues<NotificationChannel>())
    notificationFactory.Create(channel).Send("a@student.dthu.edu.vn", $"Thử kênh {channel}");

Console.WriteLine("== 5. Kiểm tra validate ==");
try
{
    clubService.CreateClub("", "x", "president@gmail.com");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"  Lỗi đúng dự kiến: {ex.Message}");
}
