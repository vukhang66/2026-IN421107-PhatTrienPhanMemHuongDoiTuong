using System;

namespace EduHub.Ch01.Domain
{
    /// <summary>
    /// Đại diện cho một thành viên trong hệ thống.
    /// </summary>
    public class Member
    {
        // Kiểm tra dòng này: phải là private set, không được là init
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string StudentCode { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private Member(Guid id, string name, string email, string studentCode, DateTime joinedAt)
        {
            Id = id;
            Name = name;
            Email = email;
            StudentCode = studentCode;
            JoinedAt = joinedAt;
        }

        public static Member Create(string name, string email, string studentCode)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Email không hợp lệ hoặc thiếu ký tự '@'.", nameof(email));

            if (string.IsNullOrWhiteSpace(studentCode))
                throw new ArgumentException("Mã sinh viên không được để trống.", nameof(studentCode));

            return new Member(Guid.NewGuid(), name.Trim(), email.Trim(), studentCode.Trim(), DateTime.UtcNow);
        }

        public bool IsActive()
        {
            return JoinedAt <= DateTime.UtcNow;
        }
    }
}