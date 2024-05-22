using MongoDB.Driver;

namespace eventApp_backend.Repositories
{
    public class MongoDBRepository
    {
        public MongoClient client;
        public IMongoDatabase db;

        public MongoDBRepository()
        {
            client = new MongoClient("mongodb+srv://manuelcarrete23:km6Kl7YtSpuafRMU@eventapp.zla5dxs.mongodb.net/");

            db = client.GetDatabase("eventApp");
        }
    }
}
