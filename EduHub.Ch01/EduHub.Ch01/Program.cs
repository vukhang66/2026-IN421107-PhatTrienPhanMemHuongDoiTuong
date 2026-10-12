using System;
using System.Collections.Generic;
using EduHub.Ch01.Domain;

namespace EduHub.Ch01
{
    class Program
    {
        // Các hằng số thông tin cấu hình hiển thị (tránh magic string)
        private const string ClubName = "Tech Club";
        private const string ClubDescription = "Câu lạc bộ Công nghệ Thông tin - Đại học Đồng Tháp";
        private const string EventTitle = "Workshop AI & Modern Software Development";
        private const int EventCapacity = 50;

        static void Main(string[] args)
        {
            // Thiết lập mã hóa tiếng Việt cho Console
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PrintHeader("BẮT ĐẦU CHẠY DEMO LUỒNG NGHIỆP VỤ EDUHUB");

            try
            {
                // Bước 1: Tạo CLB "Tech Club" với mô tả phù hợp
                Console.WriteLine(">> Bước 1: Khởi tạo Câu lạc bộ...");
                Club techClub = Club.Create(ClubName, ClubDescription);
                Console.WriteLine($"[Thành công] Đã tạo CLB: {techClub.Name} - Mô tả: {techClub.Description}");
                PrintSeparator();

                // Bước 2: Thêm 3 thành viên vào CLB (dữ liệu mẫu tự tạo)
                Console.WriteLine(">> Bước 2: Thêm các thành viên vào CLB...");
                Member member1 = Member.Create("Nguyễn Văn An", "an.nv@dthu.edu.vn", "B2101001");
                Member member2 = Member.Create("Trần Thị Bình", "binh.tt@dthu.edu.vn", "B2101002");
                Member member3 = Member.Create("Lê Hoàng Cường", "cuong.lh@dthu.edu.vn", "B2101003");

                techClub.AddMember(member1);
                techClub.AddMember(member2);
                techClub.AddMember(member3);

                Console.WriteLine($"[Thành công] Đã thêm 3 thành viên. Tổng số thành viên hiện tại của CLB: {techClub.MemberCount()}");
                foreach (var m in techClub.Members)
                {
                    Console.WriteLine($"   - {m.Name} ({m.StudentCode}) - Email: {m.Email}");
                }
                PrintSeparator();

                // Bước 3: Tạo 1 sự kiện cho CLB (ví dụ: Workshop AI, capacity = 50)
                Console.WriteLine(">> Bước 3: Tạo sự kiện mới cho CLB...");
                // Đặt thời gian sự kiện cách hiện tại 7 ngày
                DateTime eventDate = DateTime.UtcNow.AddDays(7);
                Event workshopEvent = Event.Create(techClub.Id, EventTitle, eventDate, EventCapacity, isOnline: false);

                Console.WriteLine($"[Thành công] Đã tạo sự kiện: {workshopEvent.Title}");
                Console.WriteLine($"   - Thời gian diễn ra: {workshopEvent.Date:yyyy-MM-dd HH:mm:ss UTC}");
                Console.WriteLine($"   - Sức chứa tối đa: {workshopEvent.Capacity}");
                Console.WriteLine($"   - Hình thức: {(workshopEvent.IsOnline ? "Trực tuyến" : "Trực tiếp")}");
                PrintSeparator();

                // Bước 4: 2 thành viên đăng ký tham dự sự kiện -> Status = "Pending" -> gọi Confirm()
                Console.WriteLine(">> Bước 4: Thành viên đăng ký tham gia và xác nhận...");
                Registration reg1 = Registration.Create(member1.Id, workshopEvent.Id);
                Registration reg2 = Registration.Create(member2.Id, workshopEvent.Id);

                // In trạng thái ban đầu (Pending)
                Console.WriteLine($"   - Đăng ký của {member1.Name}: Trạng thái = {reg1.Status}");
                Console.WriteLine($"   - Đăng ký của {member2.Name}: Trạng thái = {reg2.Status}");

                // Thực hiện gọi Confirm()
                reg1.Confirm();
                reg2.Confirm();
                Console.WriteLine("[Thành công] Đã gọi Confirm() cho các phiếu đăng ký:");
                Console.WriteLine($"   - {member1.Name} -> Trạng thái mới: {reg1.Status}");
                Console.WriteLine($"   - {member2.Name} -> Trạng thái mới: {reg2.Status}");
                PrintSeparator();

                // Bước 5: 1 thành viên thực hiện check-in -> tạo CheckIn record
                Console.WriteLine(">> Bước 5: Thực hiện điểm danh (Check-in)...");
                // Thành viên 1 thực hiện check-in bằng mã QR
                CheckIn checkIn1 = CheckIn.Create(reg1.Id, CheckInMethod.QRCode);
                Console.WriteLine($"[Thành công] Đã ghi nhận check-in cho thành viên {member1.Name}:");
                Console.WriteLine($"   {checkIn1.Summary()}");
                PrintSeparator();

                // Bước 6: In báo cáo tham dự: số đăng ký, số check-in, danh sách tên
                Console.WriteLine(">> Bước 6: Báo cáo tổng kết sự kiện...");
                // Giả lập danh sách đăng ký và check-in trong bộ nhớ để tổng hợp báo cáo
                var registrations = new List<Registration> { reg1, reg2 };
                var checkIns = new List<CheckIn> { checkIn1 };

                Console.WriteLine($"* Tên sự kiện: {workshopEvent.Title}");
                Console.WriteLine($"* Tổng số lượt đăng ký: {registrations.Count}");
                Console.WriteLine($"* Tổng số lượt đã Check-in: {checkIns.Count}");
                Console.WriteLine("* Danh sách chi tiết tham dự:");
                Console.WriteLine($"   1. {member1.Name} ({member1.StudentCode}) - Đăng ký: {reg1.Status} - Check-in: Thành công lúc {checkIn1.CheckedInAt:HH:mm:ss}");
                Console.WriteLine($"   2. {member2.Name} ({member2.StudentCode}) - Đăng ký: {reg2.Status} - Check-in: Chưa tham gia");
                PrintSeparator();

                // Bước 7 / Bổ sung: Thử nghiệm vi phạm luật nghiệp vụ (Ví dụ: Hủy phiếu rồi cố gắng Confirm lại)
                Console.WriteLine(">> Bước 7: Kiểm tra ngoại lệ vi phạm quy tắc nghiệp vụ...");
                Registration regTest = Registration.Create(member3.Id, workshopEvent.Id);
                Console.WriteLine($"   - Tạo phiếu đăng ký mới cho {member3.Name} với trạng thái: {regTest.Status}");

                regTest.Cancel();
                Console.WriteLine($"   - Đã gọi Cancel(). Trạng thái hiện tại: {regTest.Status}");

                Console.WriteLine("   - Cố gắng gọi Confirm() cho phiếu đã bị hủy...");
                regTest.Confirm(); // Dòng này sẽ quăng ra ngoại lệ InvalidOperationException
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n[BẮT ĐƯỢC NGOẠI LỆ THÀNH CÔNG] InvalidOperationException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[LỖI KHÔNG MONG MUỐN]: {ex.Message}");
            }

            PrintHeader("KẾT THÚC CHƯƠNG TRÌNH DEMO");
        }

        private static void PrintHeader(string title)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine($" {title}");
            Console.WriteLine("==================================================");
        }

        private static void PrintSeparator()
        {
            Console.WriteLine("--------------------------------------------------");
        }
    }
}