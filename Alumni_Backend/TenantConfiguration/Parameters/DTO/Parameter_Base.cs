namespace Alumni_Portal.TenantConfiguration.Parameter.DTO
{
    public class Parameter_Base_DTO
    {
        public int ID { get; set; }
        public string Value { get; set; } = string.Empty;

        public string AlternateValue { get; set; } = string.Empty;
    }

    public class Parameter_Base_String_DTO {

        public string ID { get; set; }
        public string Value { get; set; } = string.Empty;

        public string AlternateValue { get; set; } = string.Empty;

    }
}
