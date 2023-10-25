

using MongoDB.Driver;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly IMongoCollection<Transaction> dataUser;
        private readonly string key;

        public TransactionService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Transaction>("transaction");
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
        public async Task<object> Post(TransactionDto item)
        {
            try
            {
                var TransactionData = new Transaction()
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdUser = item.IdUser,
                        IdTransactions = item.IdTransactions,
                        IdStatus = item.IdStatus,
                        PaymentAmount = item.PaymentAmount,
                        AdminFee = item.AdminFee,
                        TotalAmount = item.TotalAmount,
                        Notes = item.Notes,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                await dataUser.InsertOneAsync(TransactionData);
                return new { code = 200, id = TransactionData.Id, message = "Data Add Complete" };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<object> Put(string id, TransactionDto item)
        {
            try
            {
                var TransactionData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (TransactionData == null)
                {
                    throw new CustomException(400,"Data Not Found");
                }
                TransactionData.IsVerification = true;
                await dataUser.ReplaceOneAsync(x => x.Id == id, TransactionData);
                return new { code = 200, id = TransactionData.Id.ToString(), message = "Data Updated" };
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
                var TransactionData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (TransactionData == null)
                {
                    throw new CustomException(400,"Data Not Found");
                }
                TransactionData.IsActive = false;
                await dataUser.ReplaceOneAsync(x => x.Id == id, TransactionData);
                return new { code = 200, id = TransactionData.Id.ToString(), message = "Data Deleted" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
    }
}