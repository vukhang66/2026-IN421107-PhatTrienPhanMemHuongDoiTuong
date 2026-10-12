using System;

namespace EduHub.Ch01.Domain
{
    /// <summary>
    /// Đại diện cho bản ghi đăng ký sự kiện.
    /// </summary>
    public class Registration
    {
        public Guid Id { get; private set; }
        public Guid MemberId { get; private set; }
        public Guid EventId { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public RegistrationStatus Status { get; private set; }

        private Registration(Guid id, Guid memberId, Guid eventId, DateTime registeredAt, RegistrationStatus status)
        {
            Id = id;
            MemberId = memberId;
            EventId = eventId;
            RegisteredAt = registeredAt;
            Status = status;
        }

        /// <summary>
        /// Tạo mới thông tin đăng ký.
        /// </summary>
        public static Registration Create(Guid memberId, Guid eventId)
        {
            if (memberId == Guid.Empty)
                throw new ArgumentException("Mã thành viên không hợp lệ.", nameof(memberId));

            if (eventId == Guid.Empty)
                throw new ArgumentException("Mã sự kiện không hợp lệ.", nameof(eventId));

            return new Registration(Guid.NewGuid(), memberId, eventId, DateTime.UtcNow, RegistrationStatus.Pending);
        }

        /// <summary>
        /// Xác nhận đăng ký tham gia.
        /// </summary>
        public void Confirm()
        {
            if (Status == RegistrationStatus.Cancelled)
                throw new InvalidOperationException("Không thể xác nhận đăng ký đã bị hủy.");

            Status = RegistrationStatus.Confirmed;
        }

        /// <summary>
        /// Hủy đăng ký.
        /// </summary>
        public void Cancel()
        {
            if (Status == RegistrationStatus.Cancelled)
                throw new InvalidOperationException("Đăng ký này đã bị hủy từ trước.");

            Status = RegistrationStatus.Cancelled;
        }
    }
}