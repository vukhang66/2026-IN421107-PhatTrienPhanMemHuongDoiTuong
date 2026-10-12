using System;

namespace EduHub.Ch01.Domain
{
    /// <summary>
    /// Đại diện cho bản ghi check-in tại sự kiện.
    /// </summary>
    public class CheckIn
    {
        public Guid Id { get; private set; }
        public Guid RegistrationId { get; private set; }
        public DateTime CheckedInAt { get; private set; }
        public CheckInMethod Method { get; private set; }

        private CheckIn(Guid id, Guid registrationId, DateTime checkedInAt, CheckInMethod method)
        {
            Id = id;
            RegistrationId = registrationId;
            CheckedInAt = checkedInAt;
            Method = method;
        }

        /// <summary>
        /// Tạo mới bản ghi Check-in.
        /// </summary>
        public static CheckIn Create(Guid registrationId, CheckInMethod method)
        {
            if (registrationId == Guid.Empty)
                throw new ArgumentException("Mã đăng ký không hợp lệ.", nameof(registrationId));

            return new CheckIn(Guid.NewGuid(), registrationId, DateTime.UtcNow, method);
        }

        /// <summary>
        /// Trả về chuỗi mô tả chi tiết thông tin check-in.
        /// </summary>
        public string Summary()
        {
            return string.Format("CheckIn [Id: {0}, RegistrationId: {1}, Time: {2:yyyy-MM-dd HH:mm:ss}, Method: {3}]", Id, RegistrationId, CheckedInAt, Method);
        }
    }
}