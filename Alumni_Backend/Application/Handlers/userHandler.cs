using Microsoft.EntityFrameworkCore;
using System.Linq;
using Admin.Application.Services;
using Admin.Infrastructure.Repositories;
using Admin.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Application.Handlers
{
    public class userHandler
    {
        private IUserService _userService;

        public userHandler(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<List<userDirectoryDTO>> getUsersPaginated(string type, int _page, int _limit)
        {
               var  users = await _userService.getUsers(type)
                .Skip((_page - 1) * _limit)
                .Take(_limit)
                .ToListAsync();


            return users;
        }

        public async Task<int> createUser(NewUserDTO newUser)
        {
            try
            {
                 int id=await _userService.create(newUser);

                return id;



            }

            catch (Exception ex)
            {
                throw new Exception("Error creating user: " + ex.Message);
            }
        }











    }
}
