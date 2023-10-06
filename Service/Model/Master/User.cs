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
        public string? IdRole { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public int? PhoneNumber { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Point { get; set; }
        public string? Photo { get; set; }
        public string? PhotoKtp { get; set; }
        public string? PhotoKk { get; set; }
    }
}