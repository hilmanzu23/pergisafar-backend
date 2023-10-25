using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pergisafar.Shared.Models
{
    public class ResponseDto
    {
        [BsonElement("product_description")]
        public string? product_description { get; set; }

        [BsonElement("product_nominal")]
        public string? product_nominal { get; set; }

        [BsonElement("product_details")]
        public string? product_details { get; set; }

        [BsonElement("product_price")]
        public string? product_price { get; set; }

        [BsonElement("product_type")]
        public string? product_type { get; set; }

        [BsonElement("status")]
        public string? status { get; set; }

        [BsonElement("icon_url")]
        public string? icon_url { get; set; }
        [BsonElement("product_category")]
        public string? product_category { get; set; }
    }
}