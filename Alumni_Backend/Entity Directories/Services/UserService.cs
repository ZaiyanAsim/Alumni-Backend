using Entity_Directories.Services.DTO;
using Entity_Directories.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Entity_Directories.Repositories;
using Shared.Custom_Exceptions.ExceptionClasses;
namespace Entity_Directories.Services
{
    public class UserService
    {

        private IUserRepository _userRepo;
        private SharedRepository _sharedRepo;

        public UserService(IUserRepository userRepo, SharedRepository sharedRepo)
        {
            _userRepo = userRepo;
            _sharedRepo = sharedRepo;

        }

        public async Task<userDirectoryDTO?> GetUser(string individualInstitutionID)
        {
            return await _userRepo.GetUserByInstitutionID(individualInstitutionID);
        }

        public async Task<userDirectoryDTO?> GetUserByNumericId(int individualId)
        {
            return await _userRepo.GetUserByNumericId(individualId);
        }

        public async Task UpdateUserAsync(int individualId, UpdateUserDTO dto)
        {
            await _userRepo.UpdateAsync(individualId, dto);
        }

        public Task UpdateAcademicAsync(int academicId, UpdateAcademicDTO dto)
            => _userRepo.UpdateAcademicAsync(academicId, dto);

        public Task<int> AddAcademicAsync(int individualId, AddAcademicDTO dto)
            => _userRepo.AddAcademicAsync(individualId, dto);

        public Task DeleteAcademicAsync(int academicId)
            => _userRepo.DeleteAcademicAsync(academicId);

        public Task<List<WorkExperienceDTO>> GetWorkExperienceAsync(int individualId)
            => _userRepo.GetWorkExperienceAsync(individualId);

        public Task<int> AddWorkExperienceAsync(int individualId, AddWorkExperienceDTO dto)
            => _userRepo.AddWorkExperienceAsync(individualId, dto);

        public Task DeleteWorkExperienceAsync(int workExpId)
            => _userRepo.DeleteWorkExperienceAsync(workExpId);

        public async Task<PaginatedResult<userDirectoryDTO>> GetUsersPaginated(string type, int _page, int _limit)
        {
            if (_page > 0 && _limit > 0) {

                var query =  _userRepo.GetUsers(type);
                var totalCount = await _sharedRepo.CountAsync(query);

                var users = await query.
                    Skip((_page - 1) * _limit)
                    .Take(_limit)
                    .ToListAsync();


                return new PaginatedResult<userDirectoryDTO>
                {
                    data = users,
                    totalRecords = totalCount,
                    _page = _page,
                    _size = _limit

                };



            }

            throw new ValidationException("Page and Limit must be greater than 0");

        }
        

        public async Task CreateUser(NewUserDTO newUser)
        {
            int id = await _sharedRepo.Individual_Exists_Async(newUser.Institution_ID, newUser.Email);

            if (id != 0)
            {
                throw new ValidationException("Individual with this ID already exists");
            }
            
             await _userRepo.Create(newUser);

            return;

            
        }




        public async Task<List<int>> DeleteUsersBulk(List<int> individualIDs)
        {


            return await _userRepo.DeleteBulkAsync(individualIDs);

        }





    }
}
