using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pergisafar.Shared.Models
{
    public class ApprovalPayment
    {
        [BsonElement("merchantOrderId")]
        public string? merchantOrderId { get; set; }

        [BsonElement("signature")]
        public string? signature { get; set; }
    }
}