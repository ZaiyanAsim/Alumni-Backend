namespace Alumni_Portal.TenantConfiguration.Parameter.DTO
{
    

    
    public class ProjectDirectoryParams
    {
     public List<Parameter_Base_DTO> ProjectTypes { get; set; } = new List<Parameter_Base_DTO>();
     public List<Parameter_Base_DTO> ProjectIndustries { get; set; } = new List<Parameter_Base_DTO>();
    }


}
