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

        public async Task<string> Create(Guest newGuest)
        {
            newGuest.Id = Guid.NewGuid().ToString();
            await _guest.InsertOneAsync(newGuest);
            return newGuest.Id;
        }

        public async Task Delete(string id)
        {            
            await _guest.DeleteOneAsync(s => s.Id == id);
        }

        public async Task<Guest> Get(string id)
        {
            return await _guest.FindAsync(
                new BsonDocument { { "_id", id } }).Result.FirstAsync();
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
