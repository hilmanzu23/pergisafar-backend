

using pergisafar.Shared.Models;
using static RepositoryPattern.Services.TransactionsService.TransactionsService;

public interface ITransactionsService
{
    Task<List<TransactionsType>> Get();
    Task<Object> Post(TypeForm items);
    Task<Object> Put(string id, TypeForm items);
    Task<Object> Delete(string id);
}
