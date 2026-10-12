# Sơ đồ lớp sơ bộ (Class Diagram) - EduHub

Sơ đồ thể hiện cấu trúc domain model của hệ thống EduHub gồm 5 class chính: `Member`, `Club`, `Event`, `Registration`, và `CheckIn`[cite: 2].

```mermaid
classDiagram
    direction LR

    class Member {
        +Guid Id (PK)
        +string Name
        +string Email
        +string StudentCode
        +DateTime JoinedAt
        +bool IsActive()
    }

    class Club {
        +Guid Id (PK)
        +string Name
        +string Description
        +DateTime CreatedAt
        +List~Member~ Members
        +void AddMember(Member member)
        +int MemberCount()
    }

    class Event {
        +Guid Id (PK)
        +Guid ClubId (FK)
        +string Title
        +DateTime Date
        +int Capacity
        +bool IsOnline
        +bool HasCapacity(int registeredCount)
    }

    class Registration {
        +Guid Id (PK)
        +Guid MemberId (FK)
        +Guid EventId (FK)
        +DateTime RegisteredAt
        +RegistrationStatus Status
        +void Confirm()
        +void Cancel()
    }

    class CheckIn {
        +Guid Id (PK)
        +Guid RegistrationId (FK)
        +DateTime CheckedInAt
        +CheckInMethod Method
        +string Summary()
    }

    Club "1" --> "*" Member : chứa / quản lý
    Club "1" --> "*" Event : tổ chức
    Member "*" --> "*" Event : đăng ký tham gia (qua Registration)
    Event "1" --> "*" Registration : có các phiếu
    Member "1" --> "*" Registration : thực hiện
    Registration "1" --> "0..1" CheckIn : ghi nhận điểm danh