using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eventApp_backend.Model
{
    public class Guest
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Status { get; set; }

    }
}
