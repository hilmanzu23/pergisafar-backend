using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PaymentMaking;
namespace pergisafar.Shared.Models
{
    public class Payment : BaseModel
    {
        internal Temperatures? Data;

        [BsonId]
        public string? Id { get; set; }

        [BsonElement("IdUser")]
        public string? IdUser { get; set; }

        [BsonElement("IdTransactions")]
        public string? IdTransactions { get; set; }

        [BsonElement("Signature")]
        public string? Signature { get; set; }
        
        [BsonElement("Amount")]
        public double Amount { get; internal set; }
    }
}