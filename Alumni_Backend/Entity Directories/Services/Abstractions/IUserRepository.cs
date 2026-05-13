using Entity_Directories.Services.DTO;
using Alumni_Portal.Infrastructure.Data_Models;

namespace Entity_Directories.Services.Abstractions
{
    
        public interface IUserRepository
        {
            public IQueryable<userDirectoryDTO> GetUsers(string type);
            public Task<userDirectoryDTO?> GetUserByInstitutionID(string individualInstitutionID);
            public Task<userDirectoryDTO?> GetUserByNumericId(int individualId);
            public Task UpdateAsync(int individualId, UpdateUserDTO dto);
            public Task UpdateAcademicAsync(int academicId, UpdateAcademicDTO dto);
            public Task<int> AddAcademicAsync(int individualId, AddAcademicDTO dto);
            public Task DeleteAcademicAsync(int academicId);
            public Task<List<WorkExperienceDTO>> GetWorkExperienceAsync(int individualId);
            public Task<int> AddWorkExperienceAsync(int individualId, AddWorkExperienceDTO dto);
            public Task DeleteWorkExperienceAsync(int workExpId);
            public Task Create(NewUserDTO newUser);

        public Task<List<int>> DeleteBulkAsync(List<int> individualIds);

        }
    }

