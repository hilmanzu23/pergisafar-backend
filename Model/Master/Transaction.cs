using CheckId;
using MongoDB.Bson.Serialization.Attributes;
using pergisafar.Shared.Models;

namespace pergisafar.Shared.Models;
public class Transaction : BaseModel
{
    [BsonId]
    public string? Id { get; set; }

    [BsonElement("IdUser")]
    public string? IdUser { get; set; }

    [BsonElement("IdTransactions")]
    public string? IdTransactions { get; set; }
    
    [BsonElement("TypeTransaction")]
    public TransactionsType TypeTransaction {get; set;}

    [BsonElement("IdStatus")]
    public string? IdStatus { get; set; }
    
    [BsonElement("Status")]
    public Status? Status {get; set;}

    [BsonElement("PaymentAmount")]
    public double PaymentAmount { get; set; }

    [BsonElement("AdminFee")]
    public double AdminFee { get; set; }

    [BsonElement("TotalAmount")]
    public double TotalAmount { get; set; }

    [BsonElement("Notes")]
    public string Notes { get; set; }

}