using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using Shared.Custom_Exceptions.ExceptionClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Abstractions;
using Users.Application.DTO;
using Users.Infrastructure.Data_Models;
using Users.Infrastructure.Persistance;

namespace Users.Infrastructure.Repository
{
    internal class UserDirectory : IUserDirectory
    {


        private AppDbContext _context;

        public UserDirectory(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<userDirectoryDTO> GetUsers(string individualType)
        {
            individualType = individualType.ToLower();
            return individualType switch
            {
                "alumni" => _context.Individuals
                    .Where(i => i.Individual_Type_Value == "Alumni")
                    .Select(i => new userDirectoryDTO
                    {
                        
                        Individual_Name = i.Individual_Name,
                        Individual_Email = i.Individual_Email,
                        Individual_Current_Industry = i.Individual_Current_Industry,
                        Individual_Current_Role = i.Individual_Current_Role,

                        noMentorships = i.Individual_Mentorship_Count,
                        noSponsorships = i.Individual_Sponsorship_Count,


                        Programs = i.Academics
                            .OrderByDescending(a => a.Individual_Academic_Graduation_Year)
                            .Select(a => new ProgramInfoDTO
                            {
                                Program = a.Individual_Academic_Program_Value,
                                Graduation_Year = a.Individual_Academic_Graduation_Year,
                                Department = a.Individual_Academic_Department_Value,




                            })

                    }),


                "student" => _context.Individuals
                    .Where(i => i.Individual_Type_Value == "Student")
                    .Select(i => new userDirectoryDTO
                    {
                        
                        Individual_Name = i.Individual_Name,
                        Individual_Email = i.Individual_Email,





                        Programs = i.Academics

                            .Select(a => new ProgramInfoDTO
                            {
                                Program = a.Individual_Academic_Program_Value,
                                Graduation_Year = a.Individual_Academic_Graduation_Year,
                                Department = a.Individual_Academic_Department_Value,
                            })

                    }),





                "supervisor" => _context.Individuals
                    .Where(i => i.Individual_Type_Value == "Supervisor")
                    .Select(i => new userDirectoryDTO
                    {
                        
                        Individual_Name = i.Individual_Name,
                        Individual_Email = i.Individual_Email,




                        Programs = i.Academics

                            .Select(a => new ProgramInfoDTO
                            {

                                Department = a.Individual_Academic_Department_Value,
                                Designation = a.Individual_Academic_Designation
                            })

                    }),


                _ => throw new ValidationException("Invalid individual type. Must be 'Alumni', 'Student', or 'Supervisor'.")
            };
        }





        public async Task<int> Create(NewUserDTO newUser)
        {





            var user = new Individuals
            {
                Client_ID = 1,
                Campus_ID = 1,

                Individual_Institution_ID = newUser.Individual_Insititution_ID,
                Individual_Name = newUser.Individual_Name,
                Individual_Email = newUser.Individual_Email,
                Individual_Type_Value = newUser.Individual_Type_Value,
                Individual_Contact_Number_Primary = newUser.Individual_Contact_Number_Primary,
                Individual_Is_Alumni = newUser.Individual_Is_Alumni,


            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                _context.Individuals.Add(user);
                await _context.SaveChangesAsync();





                int generatedId = user.Individual_ID;

                if (newUser.Academic_Details != null)
                {
                    

                    await AddProgramInfo(generatedId, newUser.Academic_Details);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                await Task.CompletedTask;

                return generatedId;

            }



            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while adding the academic details to the database.", ex);

            }


        }

        public async Task AddProgramInfo(int individualId, IEnumerable<ProgramInfoDTO> academicDetails)
        {
            foreach (var programInfo in academicDetails)
            {
                var academicDetail = new Individual_Academics
                {
                    Individual_ID = individualId,
                    Individual_Academic_Student_ID = programInfo.Student_ID ?? null,
                    Individual_Academic_Program_Value = programInfo.Program ?? "N/A",
                    Individual_Academic_Department_Value = programInfo.Department ?? "N/A",
                    Individual_Academic_Enrollment_Year = programInfo.Enrollment_Year ?? null,
                    Individual_Academic_Graduation_Year = programInfo.Graduation_Year ?? null,
                    Individual_Academic_Designation = programInfo.Designation ?? "N/A",
                };
                _context.Individual_Academics.Add(academicDetail);
            }


        }


    }

}





