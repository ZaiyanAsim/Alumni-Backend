using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace Entity_Directories.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;

        public EventRepository(EventDbContext context)
        {
            _context = context;
        }

        public IQueryable<EventDirectoryDTO> GetEvents()
        {
            return _context.Events
                .OrderByDescending(e => e.Event_Date)
                .Select(e => new EventDirectoryDTO
                {
                    Event_ID = e.Event_ID,
                    Event_Name = e.Event_Name,
                    Event_Date = e.Event_Date,
                    Event_Type_Value = e.Event_Type_Value,
                    Event_Status = e.Event_Status,
                    Event_Description = e.Event_Description,
                    Event_Logo_URL = e.Event_Logo_URL,
                    Event_Image_URL = e.Event_Image_URL,
                    Created_At = e.Created_At,
                });
        }

        public async Task<int> CreateAsync(CreateEventDTO dto)
        {
            var ev = new Events
            {
                Event_Name = dto.Event_Name,
                Event_Date = dto.Event_Date,
                Event_Description = dto.Event_Description,
                Event_Type_ID = dto.Event_Type_ID,
                Event_Type_Value = dto.Event_Type_Value,
                Event_Status = dto.Event_Status ?? "Active",
                Event_Logo_URL = dto.Event_Logo_URL,
                Event_Image_URL = dto.Event_Image_URL,
                Created_At = DateTime.UtcNow,
            };

            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();
            return ev.Event_ID;
        }

        public async Task<List<UpcomingEventDTO>> GetUpcomingAsync(int count, CancellationToken ct = default)
        {
            return await _context.Events
                .AsNoTracking()
                .Where(e => e.Event_Date >= DateTime.UtcNow)
                .OrderBy(e => e.Event_Date)
                .Take(count)
                .Select(e => new UpcomingEventDTO
                {
                    Event_ID = e.Event_ID,
                    Event_Name = e.Event_Name,
                    Event_Date = e.Event_Date,
                    Event_Description = e.Event_Description,
                    Event_Logo_URL = e.Event_Logo_URL,
                    Event_Image_URL = e.Event_Image_URL,
                    Event_Type_Value = e.Event_Type_Value,
                })
                .ToListAsync(ct);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return false;
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
