using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;

public interface IUserService
{
    Task<List<User>> Get();
    Task<Object> GetId(string id);
    Task<Object> Put(string id, User items);
    Task<Object> Delete(string id);
}
