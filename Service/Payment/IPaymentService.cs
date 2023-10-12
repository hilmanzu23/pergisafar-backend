using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;
using static RepositoryPattern.Services.PaymentService.PaymentService;

public interface IPaymentService
{
    Task<Object> GetPayment();
    Task<Object> MakePayment(CreatePaymentDto createPaymentDto);

}
