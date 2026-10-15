# Phân tích vi phạm SOLID trong `ClubManager`

Các số dòng dưới đây tham chiếu tới `Ch02.Refactor/ClubManager.cs` trước khi refactor.

| Dòng | Nguyên tắc | Vi phạm | Hậu quả |
|---|---|---|---|
| 35–48 | SRP — Single Responsibility Principle | `CreateClub` vừa kiểm tra dữ liệu, tạo/lưu CLB, vừa ghi log. | Thay đổi luật đặt tên, cách lưu trữ hoặc cách ghi log đều buộc phải sửa cùng một lớp; kiểm thử từng việc riêng lẻ khó hơn. |
| 48, 79, 94, 111 | DIP — Dependency Inversion Principle | `ClubManager` gọi thẳng `File.AppendAllText` và tự chọn đường dẫn file. | Nghiệp vụ phụ thuộc vào hệ thống file cụ thể, khó thay bằng logger khác và khó kiểm thử mà không tạo file thật. |
| 50–63, 82–83 | OCP — Open/Closed Principle | Các kênh thông báo được chọn bằng `if/else` theo email và gửi trực tiếp bằng `Console.WriteLine`. | Mỗi kênh mới (ví dụ SMS hoặc Slack) đòi hỏi sửa `ClubManager`, làm tăng nguy cơ ảnh hưởng các nhánh đang chạy. |
| 67–83 | SRP — Single Responsibility Principle | `AddMemberToClub` tìm CLB, kiểm tra giới hạn thành viên, thêm thành viên, ghi log và gửi nhiều thông báo. | Nhiều lý do thay đổi bị gom vào cùng một method; mỗi hành vi khó thay thế hay kiểm thử độc lập. |
| 88–99 | SRP và OCP | `CreateEvent` vừa tạo sự kiện vừa tự gửi email cho từng thành viên bằng vòng lặp và `Console.WriteLine`. | Quy tắc thông báo bị gắn cứng với luồng tạo sự kiện; thêm người nghe hoặc đổi cách gửi buộc phải sửa logic nghiệp vụ. |
| 103–111 | SRP và OCP | `PrintClubReport` vừa tính số liệu, định dạng/in báo cáo, vừa ghi log; chỉ hỗ trợ đầu ra console. | Thêm định dạng CSV hoặc thay đổi cách xuất báo cáo làm method phình to và phải chỉnh sửa lớp quản lý CLB. |
| 18–20, 116 | ISP — Interface Segregation Principle | Lớp tự nắm giữ các danh sách cụ thể và trả về trực tiếp `List<Club>` có thể bị sửa đổi. | Nghiệp vụ gắn với cách lưu trữ trong bộ nhớ; nơi gọi có thể thay đổi dữ liệu nội bộ, và việc thay kho dữ liệu đòi hỏi sửa lớp này. |

## Hướng refactor

- Chuyển luật tên CLB và giới hạn số thành viên vào domain; tách điều phối CLB, sự kiện và báo cáo thành các service riêng.
- Để service nhận logger, repository và dịch vụ thông báo qua interface ở constructor.
- Dùng Factory chọn kênh thông báo, Decorator bổ sung log cho repository, Strategy chọn định dạng báo cáo và Observer phát thông báo khi tạo sự kiện.
