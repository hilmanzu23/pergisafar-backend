
using MongoDB.Driver;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.TransactionsService
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IMongoCollection<TransactionsType> dataUser;
        private readonly string key;

        public TransactionsService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<TransactionsType>("transactionstype");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }
        public async Task<List<TransactionsType>> Get()
        {
            var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
            return items;
        }
        public async Task<object> Post(TypeForm item)
        {
            var filter = Builders<TransactionsType>.Filter.Eq(u => u.Name, item.Name);
            var user = await dataUser.Find(filter).SingleOrDefaultAsync();
            if (user != null)
            {
                throw new Exception("Nama sudah digunakan.");
            }
            var roleData = new TransactionsType()
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

        public async Task<object> Put(string id, TypeForm item)
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

        public class TypeForm
        {
            public string? Name { get; set; }
        }
    }
}