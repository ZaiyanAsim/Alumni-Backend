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
