using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTO;
using Users.Infrastructure.Data_Models;

namespace Users.Application.Abstractions
{
    public interface IUserDirectory {
        public IQueryable<userDirectoryDTO> GetUsers(string type);
        public Task <userDirectoryDTO?> GetUserByInstitutionID(string individualInstitutionID);
        public Task<int> Create(Individuals newUser);

    }

}
