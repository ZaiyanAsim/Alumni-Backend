using Alumni_Portal.TenantConfiguration.Data_Models;
using Alumni_Portal.TenantConfiguration.Parameter.DTO;
using Microsoft.EntityFrameworkCore;
namespace Alumni_Portal.TenantConfiguration.Parameter
{

    public class ProjectParameters
    {
        private ConfigurationDbContext _context;
        
        public ProjectParameters(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<ProjectDirectoryParams> GetProjectDirectoryParameters()
        {
            var parameterGroups = await (from pv in _context.Parameter_Values
                                         join p in _context.Parameters on pv.Parameter_ID equals p.Parameter_ID
                                         where p.Parameter_Name == "Project Type" ||
                                               p.Parameter_Name == "Project_Industry"
                                         select new
                                         {
                                             ParameterName = p.Parameter_Name,
                                             ID = pv.Parameter_Value_ID,
                                             Value = pv.Parameter_Value_Name,
                                             AlternateValue=pv.Parameter_Value_Text
                                         }).ToListAsync();

            return new ProjectDirectoryParams
            {
                ProjectTypes = parameterGroups
                    .Where(x => x.ParameterName == "Project Type")
                    .Select(x => new Parameter_Base_DTO { ID = x.ID, Value = x.Value, AlternateValue=x.AlternateValue })
                    .ToList(),
                ProjectIndustries = parameterGroups
                    .Where(x => x.ParameterName == "Project_Industry")
                    .Select(x => new Parameter_Base_DTO { ID = x.ID, Value = x.Value, AlternateValue=x.AlternateValue })
                    .ToList()
            };

        }

        
    }
}
