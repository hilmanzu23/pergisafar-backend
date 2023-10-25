using MongoDB.Driver;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.PricePrepaidService
{
    public class PricePrepaidService : IPricePrepaidService
    {
        private readonly IMongoCollection<PricePrepaid> dataUser;
        private readonly string key;

        public PricePrepaidService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<PricePrepaid>("priceprepaid");
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
    }
}