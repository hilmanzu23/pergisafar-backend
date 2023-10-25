using MongoDB.Bson.Serialization.Attributes;
using pergisafar.Shared.Models;

public class TransactionDto
{
    public string? IdUser { get; set; }

    public string? IdTransactions { get; set; }

    public string? IdStatus { get; set; }

    public double PaymentAmount { get; set; }

    public double AdminFee { get; set; }

    public double TotalAmount { get; set; }

    public string Notes { get; set; }

}