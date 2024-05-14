using eventApp_backend.Model;
using eventApp_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eventApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly GuestService _guestService;

        public GuestController(GuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpPost("{eventId:length(24)}")]
        public async Task<ActionResult<Guest>> AddGuest(string eventId, Guest newGuest)
        {
            var guest = await _guestService.AddGuestAsync(eventId, newGuest);
            if (guest == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(AddGuest), new { eventId = eventId, guestId = newGuest.Id }, newGuest);
        }

        [HttpPut("{eventId:length(24)}/{guestId:length(24)}")]
        public async Task<IActionResult> UpdateGuest(string eventId, string guestId, Guest updatedGuest)
        {
            var guest = await _guestService.UpdateGuestAsync(eventId, guestId, updatedGuest);
            if (guest == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{eventId:length(24)}/{guestId:length(24)}")]
        public async Task<IActionResult> RemoveGuest(string eventId, string guestId)
        {
            var result = await _guestService.RemoveGuestAsync(eventId, guestId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
    
}
