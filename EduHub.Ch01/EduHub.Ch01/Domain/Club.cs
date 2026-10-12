using System;
using System.Collections.Generic;
using System.Linq;

namespace EduHub.Ch01.Domain
{
    /// <summary>
    /// Đại diện cho một Câu lạc bộ.
    /// </summary>
    public class Club
    {
        // Thay thế new() bằng cú pháp tường minh cho C# 7.3
        private readonly List<Member> _members = new List<Member>();

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<Member> Members => _members.AsReadOnly();

        private Club(Guid id, string name, string description, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }

        /// <summary>
        /// Tạo mới Câu lạc bộ.
        /// </summary>
        public static Club Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên câu lạc bộ không được để trống.", nameof(name));

            return new Club(Guid.NewGuid(), name.Trim(), description != null ? description.Trim() : string.Empty, DateTime.UtcNow);
        }

        /// <summary>
        /// Thêm thành viên vào câu lạc bộ.
        /// </summary>
        public void AddMember(Member member)
        {
            // Thay thế ThrowIfNull bằng kiểm tra truyền thống (tương thích C# 7.3)
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (_members.Any(m => m.Id == member.Id || m.StudentCode == member.StudentCode))
                throw new InvalidOperationException("Thành viên này đã tồn tại trong câu lạc bộ.");

            _members.Add(member);
        }

        /// <summary>
        /// Trả về số lượng thành viên hiện tại.
        /// </summary>
        public int MemberCount()
        {
            return _members.Count;
        }
    }
}