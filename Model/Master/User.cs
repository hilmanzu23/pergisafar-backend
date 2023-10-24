using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pergisafar.Shared.Models
{
    public class User : BaseModel
    {
        [BsonId]
        public string? Id { get; set; }

        [BsonElement("IdRole")]
        public string? IdRole { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("Password")]
        public string? Password { get; set; }
        [BsonElement("FullName")]
        public string? FullName { get; set; }

        [BsonElement("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("Otp")]
        public string? Otp { get; set; }

        [BsonElement("Pin")]
        public string? Pin { get; set; }

        [BsonElement("Balance")]
        public double? Balance { get; set; }

        [BsonElement("Point")]
        public decimal? Point { get; set; }

        [BsonElement("Photo")]
        public string? Photo { get; set; }

        [BsonElement("PhotoKtp")]
        public string? PhotoKtp { get; set; }

        [BsonElement("Partner")]
        public string? Partner { get; set; }

        [BsonElement("PhotoKk")]
        public string? PhotoKk { get; set; }
    }
}