
using Admin.Application.DTO;
using Admin.Application.Services;
using Admin.Infrastructure.Data;
using Admin.Infrastructure.Data_Models;
using Microsoft.EntityFrameworkCore;


namespace Admin.Infrastructure.Repositories
{
    public class userRepository : IUserService
    {

        private AppDbContext _context;

        public userRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<userDirectoryDTO> getUsers(string individualType)
        {
            return individualType switch
            {
                "Alumni" =>  _context.Individuals
                    .Where(i => i.Individual_Type_Value == "Alumni")
                    .Select(i => new userDirectoryDTO
                    {
                        Individual_Institution_ID = i.Individual_Institution_ID,
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




                            })
                            .ToList()
                    }),
                    

                "Student" =>  _context.Individuals
                    .Where(i => i.Individual_Type_Value == "Student")
                    .Select(i => new userDirectoryDTO
                    {
                        Individual_Institution_ID = i.Individual_Institution_ID,
                        Individual_Name = i.Individual_Name,
                        Individual_Email = i.Individual_Email,




                        
                        Programs = i.Academics

                            .Select(a => new ProgramInfoDTO
                            {
                                Program = a.Individual_Academic_Program_Value,
                                Graduation_Year = a.Individual_Academic_Graduation_Year,
                                Department = a.Individual_Academic_Department_Value,
                            })
                            .ToList()
                    }),
                    




                "Supervisor" =>  _context.Individuals
                    .Where(i => i.Individual_Type_Value == "Supervisor")
                    .Select(i => new userDirectoryDTO
                    {
                        Individual_Institution_ID = i.Individual_Institution_ID,
                        Individual_Name = i.Individual_Name,
                        Individual_Email = i.Individual_Email,

                        

                      
                        Programs = i.Academics

                            .Select(a => new ProgramInfoDTO
                            {

                                Department = a.Individual_Academic_Department_Value,
                                Designation = a.Individual_Academic_Designation
                            })
                            .ToList()
                    }),
                    

                _ => throw new ArgumentException($"Invalid individual type: {individualType}")
            };
        }





        public async Task<int> create(NewUserDTO newUser)
        {

            
            

            
                var user = new Individuals
                {
                    Client_ID=1,
                    Campus_ID=1,

                    Individual_Institution_ID=newUser.Individual_Insititution_ID,
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
                        foreach (var program in newUser.Academic_Details)
                        {
                            var academicDetail = new Individual_Academics
                            {
                                Individual_ID = generatedId,
                                Individual_Academic_Student_ID = program.Student_ID ?? null,
                               
                                Individual_Academic_Program_Value = program.Program ?? "N/A",
                                Individual_Academic_Department_Value = program.Department ?? "N/A",
                                Individual_Academic_Enrollment_Year = program.Enrollment_Year ?? null,
                                Individual_Academic_Graduation_Year = program.Graduation_Year ?? null,
                                Individual_Academic_Designation = program.Designation ?? "N/A",
                            };






                            _context.Individual_Academics.Add(academicDetail);
                        }
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

        }


    }



