using Entity_Directories.Services.DTO;

namespace Entity_Directories.Services.Abstractions
{
    public interface IEventRepository
    {
        IQueryable<EventDirectoryDTO> GetEvents();
        Task<int> CreateAsync(CreateEventDTO dto);
        Task<List<UpcomingEventDTO>> GetUpcomingAsync(int count, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id);
    }
}
