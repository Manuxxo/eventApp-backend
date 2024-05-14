using eventApp_backend.Model;
using MongoDB.Driver;

namespace eventApp_backend.Services
{
    public class EventService
    {
        private readonly IMongoCollection<Event> _events;

        public EventService(IEventManagementDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _events = database.GetCollection<Event>(settings.EventsCollectionName);
        }

        public async Task<List<Event>> GetAsync() =>
            await _events.Find(eventItem => true).ToListAsync();

        public async Task<Event> GetAsync(string id) =>
            await _events.Find<Event>(eventItem => eventItem.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Event newEvent) =>
            await _events.InsertOneAsync(newEvent);

        public async Task UpdateAsync(string id, Event updatedEvent) =>
            await _events.ReplaceOneAsync(eventItem => eventItem.Id == id, updatedEvent);

        public async Task RemoveAsync(string id) =>
            await _events.DeleteOneAsync(eventItem => eventItem.Id == id);
    }
}
