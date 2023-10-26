using MongoDB.Driver;
using pergisafar.Shared.Models;

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
        public async Task<Object> Get()
        {
            try
            {
                var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
                return new { code = 200, data = items, message = "Data Add Complete" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
        public async Task<object> Post(CreateSettingDto item)
        {
            try
            {
                var filter = Builders<Setting>.Filter.Eq(u => u.Key, item.Key);
                var user = await dataUser.Find(filter).SingleOrDefaultAsync();
                if (user != null)
                {
                    throw new CustomException(400, "Error", "Key sudah digunakan.");
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
                return new { code = 200, id = roleData.Id, message = "Data Add Complete" };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<object> Put(string id, CreateSettingDto item)
        {
            try
            {
                var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (roleData == null)
                {
                    throw new CustomException(400, "Error", "Data Not Found");
                }
                roleData.Key = item.Key;
                roleData.Value = item.Value;
                await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
                return new { code = 200, id = roleData.Id.ToString(), message = "Data Updated" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
        public async Task<object> Delete(string id)
        {
            try
            {
                var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (roleData == null)
                {
                    throw new CustomException(400, "Error", "Data Not Found");
                }
                roleData.IsActive = false;
                await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
                return new { code = 200, id = roleData.Id.ToString(), message = "Data Deleted" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
    }
}