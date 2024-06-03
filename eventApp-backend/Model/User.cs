using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eventApp_backend.Model
{
    public class User
    {

        [BsonId]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

}
