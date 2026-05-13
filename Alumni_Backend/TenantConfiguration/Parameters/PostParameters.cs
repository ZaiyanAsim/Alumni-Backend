
using Alumni_Portal.TenantConfiguration.Parameter.DTO;
using Microsoft.EntityFrameworkCore;
using Alumni_Portal.TenantConfiguration.Data_Models;
namespace Alumni_Portal.TenantConfiguration.Parameter
{
    public class PostParameters
    {
        private readonly ConfigurationDbContext _context;

        public PostParameters(ConfigurationDbContext context)
        {
            _context = context;
        }


        public async Task<PostDirectoryParams> GetPostDirectoryParameters()
        {
            var parameterGroups = await (from pv in _context.Parameter_Values
                                         join p in _context.Parameters on pv.Parameter_ID equals p.Parameter_ID
                                         where p.Parameter_Name == "Post Type" ||
                                         p.Parameter_Name=="Institution Events"
   
                                         select new
                                         {
                                             ParameterName = p.Parameter_Name,
                                             ID = pv.Parameter_Value_ID,
                                             Value = pv.Parameter_Value_Name,
                                             AlternateValue = pv.Parameter_Value_Text
                                         }).ToListAsync();

            return new PostDirectoryParams
            {
                PostTypes = parameterGroups
                    .Where(x => x.ParameterName == "Post Type")
                    .Select(x => new Parameter_Base_DTO { ID = x.ID, Value = x.Value, AlternateValue = x.AlternateValue })
                    .ToList(),

                InstitutionEvents = parameterGroups
                    .Where(x => x.ParameterName == "Institution Events")
                    .Select(x => new Parameter_Base_DTO { ID = x.ID, Value = x.Value, AlternateValue = x.AlternateValue })
                    .ToList(),


            };
        }

    }


}
