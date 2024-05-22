using eventApp_backend.Model;
using eventApp_backend.Model.Interface;
using eventApp_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eventApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {

        private IGuestCollection db = new GuestCollection();

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            return Ok(await db.Get(id));
        }


        [HttpGet]
        public async Task<ActionResult> GetAllGuests()
        {
            var guestItem = await db.GetAllGuests();

            if (guestItem == null)
            {
                return NotFound();
            }

            return Ok(guestItem);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Guest newGuest)
        {
            await db.Create(newGuest);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Guest updatedGuest)
        {
            var guestItem = await db.Get(id);

            if (guestItem == null)
            {
                return NotFound();
            }

            updatedGuest.Id = guestItem.Id;

            await db.Update(updatedGuest);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var guestItem = await db.Get(id);

            if (guestItem == null)
            {
                return NotFound();
            }

            await db.Delete(id);

            return Ok();
        }
    }
    
}
