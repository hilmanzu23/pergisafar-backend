using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface ISettingService
{
    Task<Object> Get();
    Task<Object> Post(CreateSettingDto items);
    Task<Object> Put(string id, CreateSettingDto items);
    Task<Object> Delete(string id);
}
