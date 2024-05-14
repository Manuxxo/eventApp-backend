using eventApp_backend.Model;
using eventApp_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eventApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> Get() =>
            await _eventService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Event>> Get(string id)
        {
            var eventItem = await _eventService.GetAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return eventItem;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> Create(Event newEvent)
        {
            await _eventService.CreateAsync(newEvent);

            return CreatedAtAction(nameof(Get), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Event updatedEvent)
        {
            var eventItem = await _eventService.GetAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            updatedEvent.Id = eventItem.Id;

            await _eventService.UpdateAsync(id, updatedEvent);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var eventItem = await _eventService.GetAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            await _eventService.RemoveAsync(eventItem.Id);

            return NoContent();
        }
    }
}
