using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Entity_Directories.Repositories.MappingExpressions;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Entity_Directories.Repositories
{


    public class UserRepository : IUserRepository
    {
        private readonly IndividualDbContext _context;

        public UserRepository(IndividualDbContext context)
        {
            _context = context;
        }

        public Task<userDirectoryDTO?> GetUserByInstitutionID(string individualInstitutionID)
        {
            return _context.Individuals
                  .AsNoTracking()
                  .Where(i => i.Individual_Institution_ID == individualInstitutionID)
                  .Select(UserMappings.AlumniMapping())
                  .FirstOrDefaultAsync();
        }

        public Task<userDirectoryDTO?> GetUserByNumericId(int individualId)
        {
            return _context.Individuals
                  .AsNoTracking()
                  .Where(i => i.Individual_ID == individualId)
                  .Select(UserMappings.ProfileMapping())
                  .FirstOrDefaultAsync();
        }

        public IQueryable<userDirectoryDTO> GetUsers(string individualType)
        {
            var query = _context.Individuals.AsNoTracking();

            return individualType.ToLower() switch
            {
                "alumni" => query
                    .Where(i => i.Individual_Type_Value == "Alumni")
                    .Select(UserMappings.AlumniMapping()),
                "student" => query
                    .Where(i => i.Individual_Type_Value == "Student")
                    .Select(UserMappings.StudentMapping()),
                "supervisor" => query
                    .Where(i => i.Individual_Type_Value == "Supervisor")
                    .Select(UserMappings.SupervisorMapping()),
                _ => throw new ValidationException("Invalid individual type. Must be 'alumni', 'student', or 'supervisor'.")
            };
        }

        public async Task UpdateAsync(int individualId, UpdateUserDTO dto)
        {
            await _context.Individuals
                .Where(i => i.Individual_ID == individualId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.Individual_Name,                   i => dto.Name            ?? i.Individual_Name)
                    .SetProperty(i => i.Individual_Email,                  i => dto.Email           ?? i.Individual_Email)
                    .SetProperty(i => i.Individual_Contact_Number_Primary, i => dto.Contact_Number  ?? i.Individual_Contact_Number_Primary)
                    .SetProperty(i => i.Individual_Current_Industry,       i => dto.Current_Industry ?? i.Individual_Current_Industry)
                    .SetProperty(i => i.Individual_Current_Role,           i => dto.Current_Role    ?? i.Individual_Current_Role)
                );
        }

        public async Task UpdateAcademicAsync(int academicId, UpdateAcademicDTO dto)
        {
            await _context.Individual_Academics
                .Where(a => a.Individual_Academic_ID == academicId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Individual_Academic_Program_Value,      a => dto.Program_Value      ?? a.Individual_Academic_Program_Value)
                    .SetProperty(a => a.Individual_Academic_Department_Value,   a => dto.Department_Value   ?? a.Individual_Academic_Department_Value)
                    .SetProperty(a => a.Individual_Academic_Enrollment_Year,    a => dto.Enrollment_Year    ?? a.Individual_Academic_Enrollment_Year)
                    .SetProperty(a => a.Individual_Academic_Graduation_Year,    a => dto.Graduation_Year    ?? a.Individual_Academic_Graduation_Year)
                    .SetProperty(a => a.Individual_Academic_Designation,        a => dto.Designation_Value  ?? a.Individual_Academic_Designation)
                    .SetProperty(a => a.Individual_Academic_Institution_Name,   a => dto.Institution_Name   ?? a.Individual_Academic_Institution_Name)
                    .SetProperty(a => a.Individual_Academic_Institution_Type,   a => dto.Institution_Type   ?? a.Individual_Academic_Institution_Type)
                    .SetProperty(a => a.Individual_Academic_Is_Ongoing,         a => dto.Is_Ongoing         ?? a.Individual_Academic_Is_Ongoing)
                );
        }

        public async Task<int> AddAcademicAsync(int individualId, AddAcademicDTO dto)
        {
            var entity = new Individual_Academics
            {
                Individual_ID                        = individualId,
                Individual_Academic_Program_Value    = dto.Program_Value,
                Individual_Academic_Department_Value = dto.Department_Value,
                Individual_Academic_Enrollment_Year  = dto.Enrollment_Year,
                Individual_Academic_Graduation_Year  = dto.Graduation_Year,
                Individual_Academic_Designation      = dto.Designation_Value,
                Individual_Academic_Institution_Name = dto.Institution_Name,
                Individual_Academic_Institution_Type = dto.Institution_Type,
                Individual_Academic_Is_Ongoing       = dto.Is_Ongoing,
            };
            _context.Individual_Academics.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Individual_Academic_ID;
        }

        public async Task DeleteAcademicAsync(int academicId)
        {
            await _context.Individual_Academics
                .Where(a => a.Individual_Academic_ID == academicId)
                .ExecuteDeleteAsync();
        }

        public async Task<List<WorkExperienceDTO>> GetWorkExperienceAsync(int individualId)
        {
            return await _context.Individual_Work_Experience
                .AsNoTracking()
                .Where(w => w.Individual_ID == individualId)
                .Select(w => new WorkExperienceDTO
                {
                    Work_Experience_ID = w.Individual_Work_Experience_ID,
                    Job_Title          = w.Individual_Work_Experience_Job_Title,
                    Company_Name       = w.Individual_Work_Experience_Company_Name,
                    Start_Date         = w.Individual_Work_Experience_Start_Date,
                    End_Date           = w.Individual_Work_Experience_End_Date,
                    Is_Current         = w.Individual_Work_Experience_Is_Current,
                })
                .ToListAsync();
        }

        public async Task<int> AddWorkExperienceAsync(int individualId, AddWorkExperienceDTO dto)
        {
            var entity = new Individual_Work_Experience
            {
                Individual_ID                          = individualId,
                Individual_Work_Experience_Job_Title   = dto.Job_Title,
                Individual_Work_Experience_Company_Name = dto.Company_Name,
                Individual_Work_Experience_Start_Date  = dto.Start_Date,
                Individual_Work_Experience_End_Date    = dto.End_Date,
                Individual_Work_Experience_Is_Current  = dto.Is_Current,
            };
            _context.Individual_Work_Experience.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Individual_Work_Experience_ID;
        }

        public async Task DeleteWorkExperienceAsync(int workExpId)
        {
            await _context.Individual_Work_Experience
                .Where(w => w.Individual_Work_Experience_ID == workExpId)
                .ExecuteDeleteAsync();
        }

        public async Task Create(NewUserDTO newUser)
        {
            try

            {
                var user= UserMappings.NewUserMapping().Compile().Invoke(newUser);
                _context.Individuals.Add(user);
                await _context.SaveChangesAsync();
                return ;
                
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the individual.", ex);
            }
        }

        public async Task<List<int>> DeleteBulkAsync(List<int> individualIds)
        {


            try
            {
                // Build parameterized query
                var parameters = individualIds.Select((id, index) =>
                    new SqlParameter($"@p{index}", id)
                ).ToArray();

                var parameterNames = string.Join(",",
                    parameters.Select(p => p.ParameterName)
                );

                // Delete and get successfully deleted IDs
                var deletedIds = await _context.Database
                    .SqlQueryRaw<int>(
                        $@"DELETE FROM Individuals
                   OUTPUT DELETED.Individual_ID
                   WHERE Individual_ID IN ({parameterNames})",
                        parameters
                    )
                    .ToListAsync();


                var failedIds = individualIds.Except(deletedIds).ToList();

                return failedIds;
            }
            catch (Exception)
            {
                // If the entire delete fails, all IDs failed
                return individualIds;
            }
        }


    }
}
