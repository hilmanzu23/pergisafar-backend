using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface IBannerService
{
    Task<Object> Get();
    Task<Object> Post(CreateBannerDto items);
    Task<Object> Put(string id, CreateBannerDto items);
    Task<Object> Delete(string id);
}
