using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.TenantConfiguration.Parameter.DTO;
using Microsoft.EntityFrameworkCore;
namespace Alumni_Portal.TenantConfiguration.Parameter
{
    public class IndividualParameters
    {
        private ConfigurationDbContext _context;
        private IndividualDbContext _individualContext;
        public IndividualParameters(ConfigurationDbContext context, IndividualDbContext individualContext)
        {
            _context = context;
            _individualContext = individualContext;
        }


        public async Task<UserDirectoryParams> GetUserDirectoryParameters()
        {
            var parameterGroups = await (from pv in _context.Parameter_Values
                                         join p in _context.Parameters on pv.Parameter_ID equals p.Parameter_ID
                                         where p.Parameter_Name == "Academic Program" ||
                                               p.Parameter_Name == "Academic Department"
                                         select new
                                         {
                                             ParameterName = p.Parameter_Name,
                                             ID = pv.Parameter_Value_ID,
                                             Value = pv.Parameter_Value_Name
                                         }).ToListAsync();

            return new UserDirectoryParams
            {
                AcademicPrograms = parameterGroups
                    .Where(x => x.ParameterName == "Academic Program")
                    .Select(x => new Parameter_Base_DTO { ID = x.ID, Value = x.Value })
                    .ToList(),
                AcademicDepartments = parameterGroups
                    .Where(x => x.ParameterName == "Academic Department")
                    .Select(x => new Parameter_Base_DTO { ID = x.ID, Value = x.Value })
                    .ToList()
            };
        }

        public async Task<List<Parameter_Base_String_DTO>?> GetSupervisorsAsync()
        {
            var supervisorList= await _individualContext.Individuals
                                      
                                     .Where(i=>i.Individual_Type_Value=="Supervisor")
                                     .Select (i=> new Parameter_Base_String_DTO
                                     {
                                         ID=i.Individual_Institution_ID,
                                         Value=i.Individual_Name

                                     }).ToListAsync ();

            return supervisorList;
        }
    }
}
