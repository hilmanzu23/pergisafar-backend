using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface IRoleService
{
    Task<Object> Get();
    Task<Object> Post(CreateRoleDto items);
    Task<Object> Put(string id, CreateRoleDto items);
    Task<Object> Delete(string id);
}
