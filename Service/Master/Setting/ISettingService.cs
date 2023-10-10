
using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.SettingService.SettingService;

public interface ISettingService
{
    Task<List<Setting>> Get();
    Task<Object> Post(SettingForm items);
    Task<Object> Put(string id, Setting items);
    Task<Object> Delete(string id);
}
