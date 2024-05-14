using eventApp_backend.Model;
using MongoDB.Driver;

namespace eventApp_backend.Services
{
    public class GuestService
    {
        private readonly IMongoCollection<Event> _events;

        public GuestService(IEventManagementDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _events = database.GetCollection<Event>(settings.EventsCollectionName);
        }

        public async Task<Guest> AddGuestAsync(string eventId, Guest newGuest)
        {
            var eventItem = await _events.Find<Event>(e => e.Id == eventId).FirstOrDefaultAsync();
            if (eventItem == null)
            {
                return null;
            }

            eventItem.Guests.Add(newGuest);
            await _events.ReplaceOneAsync(e => e.Id == eventId, eventItem);

            return newGuest;
        }

        public async Task<Guest> UpdateGuestAsync(string eventId, string guestId, Guest updatedGuest)
        {
            var eventItem = await _events.Find<Event>(e => e.Id == eventId).FirstOrDefaultAsync();
            if (eventItem == null)
            {
                return null;
            }

            var guestIndex = eventItem.Guests.FindIndex(g => g.Id == guestId);
            if (guestIndex == -1)
            {
                return null;
            }

            eventItem.Guests[guestIndex] = updatedGuest;
            await _events.ReplaceOneAsync(e => e.Id == eventId, eventItem);

            return updatedGuest;
        }

        public async Task<bool> RemoveGuestAsync(string eventId, string guestId)
        {
            var eventItem = await _events.Find<Event>(e => e.Id == eventId).FirstOrDefaultAsync();
            if (eventItem == null)
            {
                return false;
            }

            var guest = eventItem.Guests.Find(g => g.Id == guestId);
            if (guest == null)
            {
                return false;
            }

            eventItem.Guests.Remove(guest);
            await _events.ReplaceOneAsync(e => e.Id == eventId, eventItem);

            return true;
        }
    }
}

