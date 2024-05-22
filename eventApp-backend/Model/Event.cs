using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eventApp_backend.Model
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Location")]
        public string Location { get; set; }

        [BsonElement("Guests")]
        public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}
