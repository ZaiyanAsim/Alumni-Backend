using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Profiles.Individual.Services.DTO;
using System.Linq.Expressions;

namespace Alumni_Portal.Profiles.Individual.Repositories.Mapping_Expressions
{
    public class Mappings
    {
        public Expression<Func<IndividualWorkExperienceDto, Individual_Work_Experience>> WorkExperienceMapping(Individual_Work_Experience? experience)
        {
            return p => new Individual_Work_Experience
            {
                Individual_ID = p.Individual_ID,
                Individual_Work_Experience_Company_Name = p.Company_Name?? experience.Individual_Work_Experience_Company_Name,
                Individual_Work_Experience_Job_Title = p.Job_Title?? experience.Individual_Work_Experience_Job_Title,
                Individual_Work_Experience_Department = p.Department?? experience.Individual_Work_Experience_Department,
                Individual_Work_Experience_Industry = p.Industry ?? experience.Individual_Work_Experience_Industry,
                Individual_Work_Experience_Employment_Type = p.Employment_Type ?? experience.Individual_Work_Experience_Employment_Type,
                Individual_Work_Experience_Start_Date = p.Start_Date ?? experience.Individual_Work_Experience_Start_Date,
                Individual_Work_Experience_End_Date = p.End_Date ?? experience.Individual_Work_Experience_End_Date,
                Individual_Work_Experience_Is_Current = p.Is_Current ,
                Individual_Work_Experience_Location = p.Location ?? experience.Individual_Work_Experience_Location,
                Individual_Work_Experience_Description = p.Description ?? experience.Individual_Work_Experience_Description,
                Individual_Work_Experience_Skills = p.Skills ?? experience.Individual_Work_Experience_Skills
                

                };

            }
        }
    }

