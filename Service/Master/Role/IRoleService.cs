using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface IRoleService
{
    Task<List<Role>> Get();
    Task<Object> Post(RoleForm items);
    Task<Object> Put(string id, Role items);
    Task<Object> Delete(string id);
}
