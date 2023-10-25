using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface ITransactionService
{
    Task<Object> Get();
    Task<Object> GetId(string id);

    Task<Object> Post(TransactionDto items);
    Task<Object> Put(string id, TransactionDto items);
    Task<Object> Delete(string id);
}
