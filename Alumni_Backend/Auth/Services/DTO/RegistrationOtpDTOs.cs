namespace Alumni_Portal.Auth.Services.DTO
{
    /// <summary>
    /// Returned by SubmitAsync so the controller (and frontend) know
    /// whether to enter the OTP flow or the alumni pending-approval flow.
    /// </summary>
    public class SubmitRegistrationResponseDTO
    {
        public int    RequestId   { get; set; }
        public bool   RequiresOtp { get; set; }
        public string Message     { get; set; } = string.Empty;
    }

    /// <summary>
    /// Sent by the frontend when the user submits the 6-digit OTP.
    /// </summary>
    public class VerifyRegistrationOtpDTO
    {
        public int    RequestId { get; set; }
        public string Otp       { get; set; } = string.Empty;
    }

    /// <summary>
    /// Sent by the frontend when the user clicks "Resend code".
    /// </summary>
    public class ResendRegistrationOtpDTO
    {
        public int RequestId { get; set; }
    }
}