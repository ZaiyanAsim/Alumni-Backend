using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTO;

namespace Users.Application.Abstractions
{
    public interface IUserDirectory {
        public IQueryable<userDirectoryDTO> GetUsers(string type);

        public Task<int> Create(NewUserDTO newUser);

    }

}
