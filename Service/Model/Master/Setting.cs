using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pergisafar.Shared.Models
{
    public class Setting : BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }

        [BsonElement("Key")]
        public string? Key { get; set; }
        
        [BsonElement("Value")]
        public string? Value { get; set; }
    }
}