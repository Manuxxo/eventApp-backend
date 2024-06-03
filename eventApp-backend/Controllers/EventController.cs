using eventApp_backend.Model;
using eventApp_backend.Model.Interface;
using eventApp_backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;


namespace eventApp_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IEventCollection db = new EventCollection();
        private IGuestCollection _guest = new GuestCollection();

        public EventController (IConfiguration configuration)
        {
            _configuration = configuration;
        }

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

        [HttpGet("GetLastEvents")]
        public async Task<ActionResult> GetLastEvents()
        {
            var eventItem = await db.GetLastEvents(4);

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

         [HttpGet("{id}/guests")]
        public async Task<ActionResult> GetGuests(string id)
        {
            var guests = await db.GetGuests(id);
            return Ok(guests);
        }

        [HttpPost("{id}/guests")]
        public async Task<ActionResult> AddGuest(string id, [FromBody] Guest newGuest)
        {
            var idGuest = await _guest.Create(newGuest);
            newGuest.Id = idGuest;
            await db.AddGuest(id, newGuest);
            var eventItem = await db.Get(id);
            await SendConfirmationEmail(newGuest, eventItem);
            return Ok();
        }

        [HttpPost("{id}/Reminder")]
        public async Task<ActionResult> SendEventReminder(string id)
        {
            var eventItem = await db.Get(id);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }
            var guests = eventItem.Guests;

            if (guests == null || !guests.Any())
            {
                return NotFound("No guests found for this event.");
            }
            foreach (var guest in guests)
            {
                await SendEventReminderEmail(guest, eventItem);
            }
            return Ok("Reminder emails sent to all guests.");
        }

        private async Task<IActionResult> SendEventReminderEmail(Guest guest, Event eventItem)
        {
            const string subject = "Recordatorio del Evento";
            string pathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Model", "Templates", "Reminder.html");
            string pathLogo = "https://i.ibb.co/V3zGhPr/logo.png";
            string eventLink = "http://localhost:4200/" + eventItem.Id + "/event-page/" + guest.Id;

            string body = await System.IO.File.ReadAllTextAsync(pathTemplate);
            body = body.Replace("{{name}}", guest.Name)
                       .Replace("{{email}}", guest.Email)
                       .Replace("{{eventName}}", eventItem.Name)
                       .Replace("{{eventDate}}", eventItem.Date.ToString("D"))
                       .Replace("{{logo}}", pathLogo)
                       .Replace("{{eventLink}}", eventLink)
                       .Replace("{{eventTime}}", eventItem.Date.ToString("t"));

            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(guest.Email);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return Ok("Reminder email sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("{id}/attendance/{guestId}")]
        public async Task<IActionResult> RegisterAttendance(string id, string guestId)
        {
            var eventItem = await db.Get(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var guest = eventItem.Guests.FirstOrDefault(g => g.Id == guestId);
            if (guest == null)
            {
                return NotFound();
            }

            guest.Status = "Attended";

            await db.Update(eventItem);

            return Ok();
        }

        [HttpDelete("{id}/guests/{guestId}")]
        public async Task<IActionResult> DeleteGuest(string id, string guestId)
        {
            await db.DeleteGuest(id, guestId);
            return Ok();
        }

        private async Task<IActionResult> SendConfirmationEmail(Guest guest, Event eventItem)
        {
            const string subject = "Confirmación de Compra de Entrada";
            string pathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Model", "Templates", "Purchase.html");
            string pathLogo = "https://i.ibb.co/V3zGhPr/logo.png";
            string eventLink = "http://localhost:4200/" + eventItem.Id + "/event-page/" + guest.Id;

            string body = await System.IO.File.ReadAllTextAsync(pathTemplate);
            body = body.Replace("{{name}}", guest.Name)
                       .Replace("{{email}}", guest.Email)
                       .Replace("{{eventName}}", eventItem.Name)
                       .Replace("{{eventDate}}", eventItem.Date.ToString("D"))
                       .Replace("{{logo}}", pathLogo)
                       .Replace("{{eventLink}}", eventLink)
                       .Replace("{{eventTime}}", eventItem.Date.ToString("t"));

            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };            
            {
                mailMessage.To.Add(guest.Email);

                smtpClient.Send(mailMessage);

                return Ok();
            }
        }
    }
}
