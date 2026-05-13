using System.Text.Json.Serialization;

namespace Alumni_Portal.Auth.Services.DTO
{
    public class AdminAuthInfoResponseDTO
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("data")]
        public AdminAuthInfoDataDTO? Data { get; set; }
    }

    public class AdminAuthInfoDataDTO
    {
        [JsonPropertyName("user_ID")]
        public int User_ID { get; set; }

        [JsonPropertyName("user_Name")]
        public string? User_Name { get; set; }

        [JsonPropertyName("user_Email")]
        public string? User_Email { get; set; }

        [JsonPropertyName("user_Designation")]
        public string? User_Designation { get; set; }

        [JsonPropertyName("user_OTP_Is_Set")]
        public bool User_OTP_Is_Set { get; set; }

        [JsonPropertyName("client_ID")]
        public int Client_ID { get; set; }

        [JsonPropertyName("client_Reference_Key")]
        public string? Client_Reference_Key { get; set; }

        [JsonPropertyName("department_ID")]
        public int? Department_ID { get; set; }

        [JsonPropertyName("department_Code")]
        public string? Department_Code { get; set; }

        [JsonPropertyName("department_Initials")]
        public string? Department_Initials { get; set; }

        [JsonPropertyName("department_Value")]
        public string? Department_Value { get; set; }

        [JsonPropertyName("group_Based_Flag")]
        public bool Group_Based_Flag { get; set; }

        [JsonPropertyName("status_ID")]
        public int Status_ID { get; set; }

        [JsonPropertyName("status_Value")]
        public string? Status_Value { get; set; }

        [JsonPropertyName("featurePermissionKeys")]
        public List<string> FeaturePermissionKeys { get; set; } = new();

        [JsonPropertyName("reportPermissionKeys")]
        public List<string> ReportPermissionKeys { get; set; } = new();

        [JsonPropertyName("userSecurityGroupName")]
        public List<string> UserSecurityGroupName { get; set; } = new();
    }
}
