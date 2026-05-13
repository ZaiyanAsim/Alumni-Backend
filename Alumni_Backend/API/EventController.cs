using Entity_Directories.Services;
using Entity_Directories.Services.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Route("api/Admin/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(int _page = 1, int _size = 20)
        {
            var result = await _eventService.GetEventsPaginated(_page, _size);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _eventService.GetAllEvents();
            return Ok(result);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming([FromQuery] int count = 5, CancellationToken ct = default)
        {
            var result = await _eventService.GetUpcomingEvents(count, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int id = await _eventService.CreateEvent(dto);
            return Ok(new { Status = "Success", Message = "Event created.", Event_ID = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            bool deleted = await _eventService.DeleteEvent(id);
            if (!deleted) return NotFound();
            return Ok(new { Status = "Success", Message = "Event deleted." });
        }
    }
}
