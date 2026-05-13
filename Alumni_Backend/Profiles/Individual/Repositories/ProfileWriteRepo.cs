using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Profiles.Individual.Services.DTO;
using Alumni_Portal.Profiles.Individual.Repositories.Mapping_Expressions;
using Alumni_Portal.Infrastructure.Data_Models;

namespace Alumni_Portal.Profiles.Individual.Repositories
{
    public class ProfileWriteRepo
    {
       private IndividualDbContext _individualDbContext;
        public ProfileWriteRepo(IndividualDbContext individualDbContext)
        {
            _individualDbContext = individualDbContext;
        }


        //public async Task AddPersonalInfoAsync(PersonalInfoDto personalInfoDto)
        //{
        //    var mapper = new Mappings().PersonalInfoMapping(null).Compile();
        //    var personalInfoEntity = mapper(personalInfoDto);
        //    _individualDbContext.Individual_Personal_Info.Add(personalInfoEntity);
        //    await _individualDbContext.SaveChangesAsync();
        //}

        public async Task AddWorkExperienceAsync(List<IndividualWorkExperienceDto> individualWorkExperienceDto, int individualId)
        {
            var mapper = new Mappings().WorkExperienceMapping(null).Compile();

            var workExperiences = individualWorkExperienceDto.Select(dto =>
            {
                var entity = mapper(dto);
                entity.Individual_ID = individualId;
                return entity;
            }).ToList();

            await _individualDbContext.Individual_Work_Experience.AddRangeAsync(workExperiences);
            await _individualDbContext.SaveChangesAsync();
        }

        
        

       
    }
}
