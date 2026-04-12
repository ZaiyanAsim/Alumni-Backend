using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Profiles.Individual.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Alumni_Portal.Profiles.Individual.Repositories.Mapping_Expressions;

namespace Alumni_Portal.Profiles.Individual.Repositories
{
    public class ProfileUpdateRepo
    {
        private IndividualDbContext _context;

        public ProfileUpdateRepo(IndividualDbContext context)
        {
            _context = context;
        }


        public async Task UpdateWorkExperienceAsync(int workExperienceId, IndividualWorkExperienceDto details)
        {
            var workExperience = await _context.Individual_Work_Experience
                 .FirstOrDefaultAsync(x =>
         x.Individual_ID == details.Individual_ID &&
         x.Individual_Work_Experience_ID == workExperienceId);

            if (workExperience == null)
            {
                throw new Exception("Work experience not found");
            }

            var mapper = new Mappings().WorkExperienceMapping(workExperience).Compile();
            var updatedWorkExperience = mapper(details);

        }
    }
}
