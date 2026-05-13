using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTO;
using Users.Infrastructure.Data_Models;

namespace Users.Infrastructure.Repository
{
    internal class MappingExpressions
    {
        public static Expression<Func<Individuals, userDirectoryDTO>> AlumniMapping()
        {

            return
                  i => new userDirectoryDTO
                  {
                      Individual_Name = i.Individual_Name,
                      Individual_Email = i.Individual_Email,
                      Individual_Current_Industry = i.Individual_Current_Industry,
                      Individual_Current_Role = i.Individual_Current_Role,
                      noMentorships = i.Individual_Mentorship_Count,
                      noSponsorships = i.Individual_Sponsorship_Count,
                      Programs = i.Academic_Details
                          .OrderByDescending(a => a.Individual_Academic_Graduation_Year)
                          .Select(a => new ProgramInfoDTO
                          {
                              Program = a.Individual_Academic_Program_Value,
                              Graduation_Year = a.Individual_Academic_Graduation_Year,
                              Department = a.Individual_Academic_Department_Value,
                          })
                  };
        }

        public static Expression<Func<Individuals, userDirectoryDTO>> StudentMapping()
        {
            return i => new userDirectoryDTO

            {
                Individual_Name = i.Individual_Name,
                Individual_Email = i.Individual_Email,
                Programs = i.Academic_Details
                            .Select(a => new ProgramInfoDTO
                            {
                                Program = a.Individual_Academic_Program_Value,
                                Graduation_Year = a.Individual_Academic_Graduation_Year,
                                Department = a.Individual_Academic_Department_Value,
                            })
            };

        }

        public static Expression<Func<Individuals, userDirectoryDTO>> SupervisorMapping()
        {
            return i => new userDirectoryDTO

            {
                Individual_Name = i.Individual_Name,
                Individual_Email = i.Individual_Email,
                Programs = i.Academic_Details
                            .Select(a => new ProgramInfoDTO
                            {
                                Department = a.Individual_Academic_Department_Value,
                                Designation = a.Individual_Academic_Designation,
                            })
            };

        }


    }
}
