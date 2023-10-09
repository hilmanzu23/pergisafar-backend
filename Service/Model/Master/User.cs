using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pergisafar.Shared.Models
{
    public class User : BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }

        [BsonElement("IdRole")]
        public ObjectId? IdRole { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("Password")]
        public string? Password { get; set; }
        [BsonElement("FullName")]
        public string? FullName { get; set; }

        [BsonElement("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("Balance")]
        public decimal? Balance { get; set; }

        [BsonElement("Point")]
        public decimal? Point { get; set; }

        [BsonElement("Photo")]
        public string? Photo { get; set; }

        [BsonElement("PhotoKtp")]
        public string? PhotoKtp { get; set; }

        [BsonElement("PhotoKk")]
        public string? PhotoKk { get; set; }
    }
}