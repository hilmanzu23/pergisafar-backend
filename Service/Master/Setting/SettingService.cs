using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;
using static test_blazor.Server.Controllers.RoleController;

namespace RepositoryPattern.Services.SettingService
{
    public class SettingService : ISettingService
    {
        private readonly IMongoCollection<Setting> dataUser;
        private readonly string key;

        public SettingService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Setting>("setting");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }
        public async Task<List<Setting>> Get()
        {
            var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
            return items;
        }
        public async Task<object> Post(SettingForm item)
        {
            var filter = Builders<Setting>.Filter.Eq(u => u.Key, item.Key);
            var user = await dataUser.Find(filter).SingleOrDefaultAsync();
            if (user != null)
            {
                throw new Exception("Nama sudah digunakan.");
            }
            var roleData = new Setting()
                {
                    Id = Guid.NewGuid().ToString(),
                    Key = item.Key,
                    Value = item.Value,
                    IsActive = true,
                    IsVerification = false,
                    CreatedAt = DateTime.Now
                };
            await dataUser.InsertOneAsync(roleData);
            return new { success = true, data = "berhasil" };
        }

        public async Task<object> Put(string id, Setting item)
        {
            
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            roleData.Key = item.Key;
            roleData.Value = item.Value;
            await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
            return new { success = true, id = roleData.Id.ToString() };
        }
        public async Task<object> Delete(string id)
        {
            
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            roleData.IsActive = true;
            await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
            return new { success = true, id = roleData.Id.ToString() };
        }

        public class SettingForm
        {
            public string? Key { get; set; }
            public string? Value { get; set; }

        }
    }
}