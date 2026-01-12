using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTO;
using Users.Infrastructure.Repository;
using Users.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Users.Application.Handlers
{
    public class DirectoryHandler
    {
        private IUserDirectory _userService;
        public DirectoryHandler(IUserDirectory userService)
        {
            _userService = userService;
        }


        public async Task<List<userDirectoryDTO>> GetUsersPaginated(string type, int _page, int _limit)
        {
            
            
                var users = await _userService.GetUsers(type)
                 //.Skip((_page - 1) * _limit)
                 //.Take(_limit)
                 .ToListAsync();


                return users;
            

            
        }

        public async Task<int> CreateUser(NewUserDTO newUser)
        {
            try
            {
                int id = await _userService.Create(newUser);

                return id;



            }

            catch (Exception ex)
            {
                throw new Exception("Error creating user: " + ex.Message);
            }
        }











    }
}

