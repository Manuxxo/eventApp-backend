using eventApp_backend.Model.Interface;
using eventApp_backend.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eventApp_backend.Repositories
{
    public class GuestCollection : IGuestCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();
        private IMongoCollection<Guest> _guest;

        public GuestCollection()
        {
            _guest = _repository.db.GetCollection<Guest>("Guest");
        }

        public async Task Create(Guest newGuest)
        {
            await _guest.InsertOneAsync(newGuest);
        }

        public async Task Delete(string id)
        {
            var filter = Builders<Guest>.Filter.Eq(s => s.Id, new ObjectId(id));
            await _guest.DeleteOneAsync(filter);
        }

        public async Task<Guest> Get(string id)
        {
            return await _guest.FindAsync(
                new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
        }

        public async Task<List<Guest>> GetAllGuests()
        {
            return await _guest.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task Update(Guest newGuest)
        {
            var filter = Builders<Guest>.Filter.Eq(s => s.Id, newGuest.Id);
            await _guest.ReplaceOneAsync(filter, newGuest);
        }

    }
}
