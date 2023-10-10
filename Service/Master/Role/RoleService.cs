using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;
using static test_blazor.Server.Controllers.RoleController;

namespace RepositoryPattern.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IMongoCollection<Role> dataUser;
        private readonly string key;

        public RoleService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Role>("roles");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }
        public async Task<List<Role>> Get()
        {
            var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
            return items;
        }
        public async Task<object> Post(RoleForm item)
        {
            var filter = Builders<Role>.Filter.Eq(u => u.Name, item.Name);
            var user = await dataUser.Find(filter).SingleOrDefaultAsync();
            if (user != null)
            {
                throw new Exception("Nama sudah digunakan.");
            }
            var roleData = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    IsActive = true,
                    IsVerification = false,
                    CreatedAt = DateTime.Now
                };
            await dataUser.InsertOneAsync(roleData);
            return new { success = true, data = "berhasil" };
        }

        public async Task<object> Put(string id, Role item)
        {
            ;
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            roleData.Name = item.Name;
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

        public class RoleForm
        {
            public string? Name { get; set; }
        }
    }
}