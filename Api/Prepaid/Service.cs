using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.PricePrepaidService
{
    public class PricePrepaidService : IPricePrepaidService
    {
        private readonly IMongoCollection<PricePrepaid> dataUser;
        private readonly string key;
        private readonly string username;
        private readonly string sign;
        private readonly string endpointDev;

        private readonly  MongoClient client;


        public PricePrepaidService(IConfiguration configuration)
        {
            this.client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<PricePrepaid>("priceprepaid");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
            this.username = configuration.GetSection("IAKSettings")["Username"];
            // this.apidev = configuration.GetSection("IAKSettings")["Dev"];
            // this.apiprod = configuration.GetSection("IAKSettings")["Prod"];
            this.sign = configuration.GetSection("IAKSettings")["SecretKeyDevPL"];
            this.endpointDev = configuration.GetSection("IAKSettings")["EndPointDev"];
        }

        public async Task<Object> GetPulsa(string provider)
        {
            try
            {   
                string operatorName = OperatorChecker.GetOperatorNamePulsa(provider).ToLower();
                // Create filter conditions for product_type and status
                var filter = Builders<PricePrepaid>.Filter.And(
                    Builders<PricePrepaid>.Filter.Eq("product_type", "pulsa"),
                    Builders<PricePrepaid>.Filter.Eq("status", "active")
                );
                // Query the MongoDB collection with the filter conditions
                var items = await dataUser.Find(filter).SortBy(p => p.product_nominal).ToListAsync();
                // Filter the results based on product_code
                var filteredProducts = items.Where(product =>
                    product.product_code.Contains(operatorName)
                ).ToList();
                return new { code = 200, data = filteredProducts, message = "Data Add Complete", length = filteredProducts.Count };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<Object> GetData(string provider)
        {
            try
            {   
                string operatorName = OperatorChecker.GetOperatorNameData(provider).ToLower();
                // Create filter conditions for product_type and status
                var filter = Builders<PricePrepaid>.Filter.And(
                    Builders<PricePrepaid>.Filter.Eq("product_type", "data"),
                    Builders<PricePrepaid>.Filter.Eq("status", "active")
                );
                // Query the MongoDB collection with the filter conditions
                var items = await dataUser.Find(filter).SortBy(p => p.product_nominal).ToListAsync();
                // Filter the results based on product_code
                var filteredProducts = items.Where(product =>
                    product.product_code.Contains(operatorName)
                ).ToList();
                return new { code = 200, data = filteredProducts, message = "Data Add Complete", length = filteredProducts.Count };
            }
            catch (CustomException)
            {
                throw;
            }
        }
        public async Task<Object> RefreshData()
        {
            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                { "status", "all" },
                { "username", username },
                { "sign", sign },
            };
            var json = JsonConvert.SerializeObject(parameters);
            try
            {     
                client.GetDatabase("testprod").DropCollection("priceprepaid");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpointDev + "pricelist", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<YourResponseType>(responseContent);
                var pricelist = responseObject.data.pricelist;
                var transactionDataList = pricelist.Select(item => new PricePrepaid
                {
                    Id = Guid.NewGuid().ToString(),
                    product_description = item.product_description.ToLower(),
                    product_code = item.product_code,
                    product_nominal = item.product_nominal,
                    product_details = item.product_details,
                    product_price = item.product_price,
                    product_type = item.product_type,
                    status = item.status,
                    icon_url = item.icon_url,
                    product_category = item.product_category,
                    CreatedAt = DateTime.Now
                }).ToList();

                await dataUser.InsertManyAsync(transactionDataList);
                return new { code = 200, message = "Berhasil" };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public class YourResponseType
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public List<PricePrepaid> pricelist { get; set; }
        }
    }
}