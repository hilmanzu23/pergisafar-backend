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

    public class CreatePaymentDto
        {
            public string PaymentMethod { get; set; }
            public string PhoneNumber { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public double PaymentAmount { get; set; }
            public string ProductDetails { get; set; }
            public string IdUser { get; set; }
        }
}