using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;
using static test_blazor.Server.Controllers.RoleController;

public interface IPricePrepaidService
{
    Task<Object> RefreshData();
    Task<Object> GetPulsa(string provider);
    Task<Object> GetData(string provider);


}
