using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface IBannerService
{
    Task<Object> Get();
    Task<Object> Post(ImageUploadViewModel item);
    Task<Object> Put(string id, ImageUploadViewModel items);
    Task<Object> Delete(string id);
}
