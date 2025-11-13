using MongoDB.Driver;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.TransactionsTypeService
{
    public class TransactionsTypeService : ITransactionsTypeService
    {
        private readonly IMongoCollection<TransactionsType> dataUser;
        private readonly string key;

        public TransactionsTypeService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("travelberkah");
            dataUser = database.GetCollection<TransactionsType>("transactionstype");
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
                var filter = Builders<TransactionsType>.Filter.Eq(u => u.Name, item.Name);
                var user = await dataUser.Find(filter).SingleOrDefaultAsync();
                if (user != null)
                {
                    throw new CustomException(400, "Error", "Nama sudah digunakan.");
                }
                var TransactionsTypeData = new TransactionsType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    IsActive = true,
                    IsVerification = false,
                    CreatedAt = DateTime.Now
                };
                await dataUser.InsertOneAsync(TransactionsTypeData);
                return new { code = 200, id = TransactionsTypeData.Id, message = "Data Add Complete" };
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
                var TransactionsTypeData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (TransactionsTypeData == null)
                {
                    throw new CustomException(400, "Error", "Data Not Found");
                }
                TransactionsTypeData.Name = item.Name;
                await dataUser.ReplaceOneAsync(x => x.Id == id, TransactionsTypeData);
                return new { code = 200, id = TransactionsTypeData.Id.ToString(), message = "Data Updated" };
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
                var TransactionsTypeData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (TransactionsTypeData == null)
                {
                    throw new CustomException(400, "Error", "Data Not Found");
                }
                TransactionsTypeData.IsActive = false;
                await dataUser.ReplaceOneAsync(x => x.Id == id, TransactionsTypeData);
                return new { code = 200, id = TransactionsTypeData.Id.ToString(), message = "Data Deleted" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
    }
}