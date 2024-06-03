using eventApp_backend.Model;
using eventApp_backend.Model.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eventApp_backend.Repositories
{
    public class EventCollection : IEventCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();
        private IMongoCollection<Event> _events;

        public EventCollection() 
        {
            _events = _repository.db.GetCollection<Event>("Event");
        }

        public async Task Create(Event newEvent)
        {
            newEvent.Id = Guid.NewGuid().ToString();
            await _events.InsertOneAsync(newEvent);
        }

        public async Task Delete(string id)
        {
            await _events.DeleteOneAsync(s => s.Id == id);
        }

        public async Task<Event> Get(string id)
        {
            return await _events.FindAsync(
                new BsonDocument { { "_id", id } }).Result.FirstAsync();
        }

        public async Task<List<Event>> GetAllEvents()
        {
            return await _events.FindAsync(new BsonDocument()).Result.ToListAsync();
        }
        public async Task<List<Event>> GetLastEvents(int limit = 4)
        {
            var sortDefinition = Builders<Event>.Sort.Descending(e => e.Date);
            var filter = Builders<Event>.Filter.Empty;
            var options = new FindOptions<Event>
            {
                Sort = sortDefinition,
                Limit = limit
            };

            using (var cursor = await _events.FindAsync(filter, options))
            {
                return await cursor.ToListAsync();
            }
        }


        public async Task Update(Event newEvent)
        {
            var filter = Builders<Event>.Filter.Eq(s => s.Id, newEvent.Id);
            await _events.ReplaceOneAsync(filter, newEvent);
        }

        public async Task<List<Guest>> GetGuests(string eventId)
        {
            var eventItem = await Get(eventId);
            return eventItem.Guests;
        }

        public async Task AddGuest(string eventId, Guest newGuest)
        {
            var eventItem = await Get(eventId);
            eventItem.Guests.Add(newGuest);
            await Update(eventItem);
        }
        
        public async Task AddAttendance(string attendance)
        {/*
            var filter = Builders<Event>.Filter.Eq(e => e.Id, attendance.EventId);
            var update = Builders<Event>.Update.Push(e => e.Attendances, attendance);
            await _events.UpdateOneAsync(filter, update);*/
        }


        public async Task UpdateGuest(string eventId, Guest updatedGuest)
        {
            var eventItem = await Get(eventId);
            var guestIndex = eventItem.Guests.FindIndex(g => g.Id == updatedGuest.Id);
            if (guestIndex != -1)
            {
                eventItem.Guests[guestIndex] = updatedGuest;
                await Update(eventItem);
            }
        }

        public async Task DeleteGuest(string eventId, string guestId)
        {
            var eventItem = await Get(eventId);
            eventItem.Guests.RemoveAll(g => g.Id == guestId);
            await Update(eventItem);
        }
    }
}
