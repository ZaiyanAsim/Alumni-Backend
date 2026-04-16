using Entity_Directories.Services.DTO;
using Alumni_Portal.Infrastructure.Data_Models;

namespace Entity_Directories.Services.Abstractions
{
    
        public interface IUserRepository
        {
            public IQueryable<userDirectoryDTO> GetUsers(string type);
            public Task<userDirectoryDTO?> GetUserByInstitutionID(string individualInstitutionID);
            public Task Create(NewUserDTO newUser);

        public Task<List<int>> DeleteBulkAsync(List<int> individualIds);

        }
    }

