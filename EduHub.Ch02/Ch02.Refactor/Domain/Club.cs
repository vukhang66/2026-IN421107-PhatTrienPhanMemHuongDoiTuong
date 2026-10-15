namespace EduHub.Ch02.Domain;

/// <summary>Câu lạc bộ. Quy tắc nghiệp vụ nên nằm ở đây, không nằm trong ClubManager.</summary>
public class Club
{
    public const int MaxMembers = 50;
    public const int MaxNameLength = 100;

    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    /// <summary>Tạo CLB mới, kiểm tra tên bắt buộc và không quá 100 ký tự.</summary>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Club name is required", nameof(name));
        if (name.Length > MaxNameLength)
            throw new ArgumentException("Club name too long", nameof(name));

        return new Club
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }

    /// <summary>CLB còn nhận thêm thành viên nếu chưa đạt giới hạn.</summary>
    public bool HasRoom(int currentMemberCount) => currentMemberCount < MaxMembers;
}
