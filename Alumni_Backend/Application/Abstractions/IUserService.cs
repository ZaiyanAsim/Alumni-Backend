using Admin.Application.DTO;

namespace Admin.Application.Services
{
    public interface IUserService
    {
        public IQueryable<userDirectoryDTO> getUsers(string type);
        
        public Task<int> create(NewUserDTO newUser);
        
    }
}
