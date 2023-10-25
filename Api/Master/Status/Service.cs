using MongoDB.Driver;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.StatusService
{
    public class StatusService : IStatusService
    {
        private readonly IMongoCollection<Status> dataUser;
        private readonly string key;

        public StatusService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Status>("Status");
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
        public async Task<object> Post(CreateRoleDto item)
        {
            try
            {
                var filter = Builders<Status>.Filter.Eq(u => u.Name, item.Name);
                var user = await dataUser.Find(filter).SingleOrDefaultAsync();
                if (user != null)
                {
                    throw new CustomException(400, "Nama sudah digunakan.");
                }
                var StatusData = new Status()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item.Name,
                        IsActive = true,
                        IsVerification = false,
                        CreatedAt = DateTime.Now
                    };
                await dataUser.InsertOneAsync(StatusData);
                return new { code = 200, id = StatusData.Id, message = "Data Add Complete" };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<object> Put(string id, CreateRoleDto item)
        {
            try
            {
                var StatusData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (StatusData == null)
                {
                    throw new CustomException(400,"Data Not Found");
                }
                StatusData.Name = item.Name;
                await dataUser.ReplaceOneAsync(x => x.Id == id, StatusData);
                return new { code = 200, id = StatusData.Id.ToString(), message = "Data Updated" };
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
                var StatusData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (StatusData == null)
                {
                    throw new CustomException(400,"Data Not Found");
                }
                StatusData.IsActive = false;
                await dataUser.ReplaceOneAsync(x => x.Id == id, StatusData);
                return new { code = 200, id = StatusData.Id.ToString(), message = "Data Deleted" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
    }
}