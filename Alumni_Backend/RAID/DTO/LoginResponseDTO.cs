namespace Alumni_Portal.RAID.Login.DTO
{
    public class LoginResponseDto
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        public LoginDataDto data { get; set; }
        public int total { get; set; }
    }

    public class LoginDataDto
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime accessTokenExpiry { get; set; }
        public DateTime refreshTokenExpiry { get; set; }
    }

}
