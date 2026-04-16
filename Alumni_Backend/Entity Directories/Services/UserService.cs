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
            int id = await _sharedRepo.Individual_Exists_Async(newUser.Institution_ID);

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
