using System.Linq.Expressions;

using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services.DTO;

namespace Entity_Directories.Repositories.MappingExpressions
{
    public  class UserMappings
    {
    
        public static Expression<Func<Individuals, userDirectoryDTO>> AlumniMapping()
        {

            return i => new userDirectoryDTO
            {
                Individual_ID=i.Individual_ID,
                Individual_Academic_ID=i.Individual_Institution_ID??"N/A",
                Individual_Name = i.Individual_Name,
                Individual_Email = i.Individual_Email,
                Individual_Current_Industry = i.Individual_Current_Industry,
                Individual_Current_Role = i.Individual_Current_Role,
                noMentorships = i.Individual_Mentorship_Count,
                noSponsorships = i.Individual_Sponsorship_Count,
                Program = i.Academic_Details
               .OrderByDescending(a => a.Individual_Academic_Graduation_Year)
               .Select(a => new ProgramInfoDTO
               {
                   Program = a.Individual_Academic_Program_Value??"N/A",
                   Graduation_Year = a.Individual_Academic_Graduation_Year,
                   Department = a.Individual_Academic_Department_Value,
                   
               }).FirstOrDefault()

            };
        }

        public static Expression<Func<Individuals, userDirectoryDTO>> StudentMapping()
        {
            return i => new userDirectoryDTO

            {
                Individual_ID = i.Individual_ID,
                Individual_Academic_ID = i.Individual_Institution_ID??"N/A",
                Individual_Name = i.Individual_Name,
                Individual_Email = i.Individual_Email,
                Program = i.Academic_Details
                            .Select(a => new ProgramInfoDTO
                            {
                                Program = a.Individual_Academic_Program_Value??"N/A",
                                Graduation_Year = a.Individual_Academic_Graduation_Year,
                                Department = a.Individual_Academic_Department_Value,
                            }).FirstOrDefault(),
            };

        } 

        public static Expression<Func<Individuals, userDirectoryDTO>> SupervisorMapping()
        {
            return i => new userDirectoryDTO

            {
                Individual_ID = i.Individual_ID,
                Individual_Academic_ID = i.Individual_Institution_ID?? "N/A",
                Individual_Name = i.Individual_Name,
                Individual_Email = i.Individual_Email,
                Program = i.Academic_Details
                            .Select(a => new ProgramInfoDTO
                            {
                                Department = a.Individual_Academic_Department_Value,
                                Designation = a.Individual_Academic_Designation,
                            }).FirstOrDefault()
            };

        }

        public static Expression<Func<NewUserDTO, Individuals>> NewUserMapping()
        {
            return n => new Individuals
            {
                Individual_Institution_ID = n.Institution_ID,
                Individual_Name = n.Name,
                Individual_Email = n.Email,
                Individual_Type_ID = n.Type_ID,
                Individual_Type_Value = n.Type_Value,
                Campus_ID = n.Campus_ID,
                Client_ID = n.Client_ID,
                Individual_Is_Alumni = n.Is_Alumni,
                Individual_Contact_Number_Primary = n.Contact_Number,

                // Only map Academic_Details if it exists and has items
                Academic_Details = n.Academic_Details.Any() == true
                ? n.Academic_Details.Select(a => new Individual_Academics
            {
                Individual_Academic_Program_ID = a.Program_ID,
                Individual_Academic_Program_Value = a.Program_Value,
                Individual_Academic_Department_ID = a.Department_ID,
                Individual_Academic_Department_Value = a.Department_Value,
                Individual_Academic_Enrollment_Year = a.Enrollment_Year,
                Individual_Academic_Graduation_Year = a.Graduation_Year,
                Individual_Academic_Designation = a.Designation_Value,
                Individual_Academic_Designation_ID = a.Designation_ID,
             }).ToList()
            : null  
            };



        }


    }
}
