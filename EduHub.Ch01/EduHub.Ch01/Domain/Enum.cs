namespace EduHub.Ch01.Domain
{
    /// <summary>
    /// Trạng thái của một đăng ký tham gia sự kiện.
    /// </summary>
    public enum RegistrationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    /// <summary>
    /// Hình thức check-in sự kiện.
    /// </summary>
    public enum CheckInMethod
    {
        QRCode,
        Manual,
        NFC
    }
}