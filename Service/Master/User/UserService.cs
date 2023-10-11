using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;

namespace RepositoryPattern.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> dataUser;
        private readonly string key;

        public UserService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<User>("users");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }
        public async Task<List<User>> Get()
        {
            var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
            return items;
        }

        public async Task<object> Put(string id, User item)
        {
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            roleData.FullName = item.FullName;
            roleData.PhoneNumber = item.PhoneNumber;
            roleData.Balance = item.Balance;
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

        public async Task<Object> GetId(string id)
        {
            var items = await dataUser.Find(_ => _.Id == id).FirstOrDefaultAsync();
            if (items == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            return items;
        }
    }
}