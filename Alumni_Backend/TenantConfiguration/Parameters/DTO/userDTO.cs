namespace Alumni_Portal.TenantConfiguration.Parameter.DTO
{
    


    public class UserDirectoryParams
    {
        public List<Parameter_Base_DTO> AcademicPrograms { get; set; } = new List<Parameter_Base_DTO>();
        public List<Parameter_Base_DTO> AcademicDepartments { get; set; } = new List<Parameter_Base_DTO>();

        public List<Parameter_Base_DTO> AcademicDesignations { get; set; } = new List<Parameter_Base_DTO>();
    }

}
