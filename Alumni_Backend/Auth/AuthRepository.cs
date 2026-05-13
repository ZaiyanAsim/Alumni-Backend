using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
namespace Alumni_Portal.Auth
{
    public class AuthRepository
    {
        private IndividualDbContext _context;

        public AuthRepository(IndividualDbContext context)
        {
            _context = context;
        }

        public async Task<Individuals?> GetByEmailAsync(string email)
        {
          return await _context.Individuals
                .FirstOrDefaultAsync(i => i.Individual_Email == email);
        }

        public async Task EnterDummyPasswords(string password, string id)
        {
            var individual = await _context.Individuals
                .FirstOrDefaultAsync(i => i.Individual_Institution_ID == id);
            if (individual != null)
            {
                individual.password_hash = password;
                await _context.SaveChangesAsync();
            }
        }

    }
}
