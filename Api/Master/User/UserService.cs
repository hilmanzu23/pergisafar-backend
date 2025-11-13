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
        private readonly IMongoCollection<Role> dataRole;

        private readonly string key;

        public UserService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("travelberkah");
            dataUser = database.GetCollection<User>("users");
            dataRole = database.GetCollection<Role>("roles");

            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }
        public async Task<List<User>> Get()
        {
            var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
            return items;
        }

        public async Task<object> Put(string id, User item)
        {
            try
            {

                var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (roleData == null)
                {
                    throw new CustomException(400, "Error", "Data NotFound");
                }
                roleData.FullName = item.FullName;
                roleData.PhoneNumber = item.PhoneNumber;
                roleData.Balance = item.Balance;
                await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
                return new { success = true, id = roleData.Id.ToString() };
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
                    throw new CustomException(400, "Error", "Data NotFound");
                }
                roleData.IsActive = true;
                await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
                return new { success = true, id = roleData.Id.ToString() };
            }
            catch (CustomException)
            {

                throw;
            }
        }

        public async Task<Object> GetId(string id)
        {
            try
            {
                var items = await dataUser.Find(_ => _.Id == id).FirstOrDefaultAsync();
                if (items == null)
                {
                    throw new CustomException(400, "Error", "Data tidak ditemukan silahkan login kembali");
                }
                var Role = await dataRole.Find(_ => _.Id == items.IdRole).FirstOrDefaultAsync();
                return new
                {
                    id = items.Id,
                    Roles = Role.Name,
                    Name = items.FullName,
                    Balance = items.Balance,
                    Point = items.Point,
                    Email = items.Email,
                    Phone = items.PhoneNumber
                };
            }
            catch (CustomException)
            {

                throw;
            }
        }
    }
}