using eventApp_backend.Model;
using eventApp_backend.Model.Interface;
using eventApp_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eventApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventCollection db = new EventCollection();

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            return Ok(await db.Get(id));
        }
            

        [HttpGet]
        public async Task<ActionResult> GetAllEvents()
        {
            var eventItem = await db.GetAllEvents();

            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Event newEvent)
        {
            await db.Create(newEvent);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id,[FromBody] Event updatedEvent)
        {
            var eventItem = await db.Get(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            updatedEvent.Id = eventItem.Id;

            await db.Update(updatedEvent);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var eventItem = await db.Get(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            await db.Delete(id);

            return Ok();
        }
    }
}
