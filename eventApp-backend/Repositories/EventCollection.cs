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
            await _events.InsertOneAsync(newEvent);
        }

        public async Task Delete(string id)
        {
            var filter = Builders<Event>.Filter.Eq(s => s.Id, new ObjectId(id));
            await _events.DeleteOneAsync(filter);
        }

        public async Task<Event> Get(string id)
        {
            return await _events.FindAsync(
                new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
        }

        public async Task<List<Event>> GetAllEvents()
        {
            return await _events.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task Update(Event newEvent)
        {
            var filter = Builders<Event>.Filter.Eq(s => s.Id, newEvent.Id);
            await _events.ReplaceOneAsync(filter, newEvent);
        }

    }
}
