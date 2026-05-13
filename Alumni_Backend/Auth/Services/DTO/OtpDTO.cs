namespace Alumni_Portal.Auth.Services.DTO
{
    


        public class VerifyRegistrationOtpDTO
        {
            public required int RequestId { get; set; }
            public required string Otp { get; set; }
        }

        public class ResendRegistrationOtpDTO
        {
            public required int RequestId { get; set; }
        }
    }

