using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pergisafar.Shared.Models
{
    public class Payment : BaseModel
    {
        [BsonId]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }
    }
}