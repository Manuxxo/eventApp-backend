using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace eventApp_backend.Model
{
    public class Event
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }
        public string Image { get; set; }

        public int Price { get; set; }

        public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}
