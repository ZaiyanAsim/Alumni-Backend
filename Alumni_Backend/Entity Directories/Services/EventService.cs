using Alumni_Portal.Entity_Directories.Repositories;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;

namespace Entity_Directories.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepo;
        private readonly SharedRepository _sharedRepo;

        public EventService(IEventRepository eventRepo, SharedRepository sharedRepo)
        {
            _eventRepo = eventRepo;
            _sharedRepo = sharedRepo;
        }

        public async Task<PaginatedResult<EventDirectoryDTO>> GetEventsPaginated(int page, int limit)
        {
            if (page < 1 || limit < 1)
                throw new ValidationException("Page and Limit must be greater than 0");

            var query = _eventRepo.GetEvents();
            int count = await _sharedRepo.CountAsync(query);
            var events = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            return new PaginatedResult<EventDirectoryDTO>
            {
                data = events,
                totalRecords = count,
                _page = page,
                _size = limit,
            };
        }

        public async Task<List<EventDirectoryDTO>> GetAllEvents()
        {
            return await _eventRepo.GetEvents().ToListAsync();
        }

        public async Task<int> CreateEvent(CreateEventDTO dto)
        {
            if (dto == null) throw new ValidationException("Event data cannot be null");
            return await _eventRepo.CreateAsync(dto);
        }

        public async Task<List<UpcomingEventDTO>> GetUpcomingEvents(int count = 5, CancellationToken ct = default)
        {
            return await _eventRepo.GetUpcomingAsync(count, ct);
        }

        public async Task<bool> DeleteEvent(int id)
        {
            return await _eventRepo.DeleteAsync(id);
        }
    }
}
