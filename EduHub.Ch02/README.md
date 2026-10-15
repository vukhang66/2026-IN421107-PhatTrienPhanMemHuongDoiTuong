# EduHub Ch02 — Refactor theo SOLID và Design Patterns

Dự án minh họa cách tách `ClubManager` (God class) thành các thành phần có trách nhiệm rõ ràng, phụ thuộc qua interface và được lắp ghép tại `Ch02.Refactor/Program.cs`.

## Cấu trúc

| Thư mục | Trách nhiệm |
|---|---|
| `Ch02.Refactor/Domain/` | Quy tắc nghiệp vụ của CLB, thành viên và sự kiện |
| `Ch02.Refactor/Services/` | Điều phối nghiệp vụ CLB, sự kiện và báo cáo |
| `Ch02.Refactor/Repositories/` | Interface kho dữ liệu, bản lưu trong bộ nhớ và Decorator ghi log |
| `Ch02.Refactor/Notifications/` | Dịch vụ thông báo và Factory chọn Email, SMS hoặc Slack |
| `Ch02.Refactor/Patterns/Observers/` | Observer ghi log và thông báo email khi tạo sự kiện |
| `Ch02.Refactor/Patterns/Strategies/` | Strategy xuất báo cáo dạng Console hoặc CSV |
| `Ch02.Refactor/Logging/` | Interface ghi log cùng cài đặt Console và file |
| `Ch02.Refactor.Tests/` | Unit test AAA cho Domain, service và bốn pattern |
| `docs/solid-analysis.md` | Phân tích các vi phạm SOLID trước khi refactor |

## Chạy

```powershell
dotnet build Ch02.Refactor.sln
dotnet test Ch02.Refactor.sln
dotnet run --project Ch02.Refactor
```

Ứng dụng lưu dữ liệu trong bộ nhớ; các thông báo và log console là mô phỏng để minh họa việc thay thế phụ thuộc. Có thể chọn `FileLogger` tại composition root nếu muốn ghi log ra file.

Ứng dụng đặt mã hóa đầu ra console là UTF-8 để hiển thị đúng tiếng Việt trong terminal.
