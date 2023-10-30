
using System.Text;
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

        private readonly MongoClient client;


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

        public async Task<Object> GetPulsa(string phone)
        {
            try
            {
                string operatorName = OperatorChecker.GetOperatorNamePulsa(phone).ToLower();
                if (operatorName == "other operator")
                {
                    throw new CustomException(400, "error", "nomor ini belum ada penawaran");
                }
                // Create filter conditions for product_type and status
                var filter = Builders<PricePrepaid>.Filter.And(
                    Builders<PricePrepaid>.Filter.Eq("product_type", "pulsa"),
                    Builders<PricePrepaid>.Filter.Eq("status", "active")
                );
                // Query the MongoDB collection with the filter conditions
                var items = await dataUser.Find(filter).ToListAsync();
                // Filter the results based on product_code
                var filteredProducts = items.Where(product => product.product_code.Contains(operatorName)).ToList();
                var sortedItems = filteredProducts.Select(p => new
                {
                    id = p.Id,
                    product_nominal = p.product_nominal,
                    product_code = p.product_code,
                    product_description = p.product_description,
                    product_details = p.product_details,
                    product_price = Convert.ToInt32(p.product_price),
                    product_type = p.product_type,
                    status = p.status,
                    icon_url = p.icon_url,
                    product_category = p.product_category,
                })
                .OrderBy(p => p.product_price)
                .ToList();
                return new { code = 200, data = sortedItems, message = "Data Add Complete", length = sortedItems.Count };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<Object> GetData(string phone)
        {
            try
            {
                string operatorName = OperatorChecker.GetOperatorNameData(phone).ToLower();
                if (operatorName == "other operator")
                {
                    throw new CustomException(400, "error", "nomor ini belum ada penawaran");
                }
                // Create filter conditions for product_type and status
                var filter = Builders<PricePrepaid>.Filter.And(
                    Builders<PricePrepaid>.Filter.Eq("product_type", "data"),
                    Builders<PricePrepaid>.Filter.Eq("status", "active")
                );
                // Query the MongoDB collection with the filter conditions
                var items = await dataUser.Find(filter).ToListAsync();
                // Filter the results based on product_code
                var filteredProducts = items.Where(product => product.product_code.Contains(operatorName)).ToList();
                var sortedItems = filteredProducts.Select(p => new
                {
                    id = p.Id,
                    product_nominal = p.product_nominal,
                    product_code = p.product_code,
                    product_description = p.product_description,
                    product_details = p.product_details,
                    product_price = Convert.ToInt32(p.product_price),
                    product_type = p.product_type,
                    status = p.status,
                    icon_url = p.icon_url,
                    product_category = p.product_category,
                })
                .OrderBy(p => p.product_price)
                .ToList();
                return new { code = 200, data = sortedItems, message = "Data Add Complete", length = sortedItems.Count };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<Object> GetPln(string notoken)
        {

            try
            {
                var item = await checkCustomerPln(notoken);
                string file = (string)item;
                // Create filter conditions for product_type and status
                var filter = Builders<PricePrepaid>.Filter.And(
                    Builders<PricePrepaid>.Filter.Eq("product_type", "pln"),
                    Builders<PricePrepaid>.Filter.Eq("status", "active")
                );
                // Query the MongoDB collection with the filter conditions
                var items = await dataUser.Find(filter).ToListAsync();
                // Filter the results based on product_code
                var filteredProducts = items.ToList();
                var sortedItems = filteredProducts.Select(p => new
                {
                    id = p.Id,
                    product_nominal = p.product_nominal,
                    product_code = p.product_code,
                    product_description = p.product_description,
                    product_details = p.product_details,
                    product_price = Convert.ToInt32(p.product_price),
                    product_type = p.product_type,
                    status = p.status,
                    icon_url = p.icon_url,
                    product_category = p.product_category,
                })
                .OrderBy(p => p.product_price)
                .ToList();
                return new { code = 200, data = sortedItems, message = "Data Add Complete", length = sortedItems.Count };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<Object> GetEmoney(string product_description)
        {
            try
            {
                // Create filter conditions for product_type and status
                var filter = Builders<PricePrepaid>.Filter.And(
                    Builders<PricePrepaid>.Filter.Eq("product_description", product_description),

                    Builders<PricePrepaid>.Filter.Eq("product_type", "etoll"),
                    Builders<PricePrepaid>.Filter.Eq("status", "active")
                );
                // Query the MongoDB collection with the filter conditions
                var items = await dataUser.Find(filter).ToListAsync();
                // Filter the results based on product_code
                var filteredProducts = items.ToList();
                var sortedItems = filteredProducts.Select(p => new
                {
                    id = p.Id,
                    product_nominal = p.product_nominal,
                    product_code = p.product_code,
                    product_description = p.product_description,
                    product_details = p.product_details,
                    product_price = Convert.ToInt32(p.product_price),
                    product_type = p.product_type,
                    status = p.status,
                    icon_url = p.icon_url,
                    product_category = p.product_category,
                })
                .OrderBy(p => p.product_price)
                .ToList();
                return new { code = 200, data = sortedItems, message = "Data Add Complete", length = sortedItems.Count };
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<Object> checkCustomerPln(string customer)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var CreateSha = new CreateSha(configuration);
            string signature = CreateSha.md5Conv(customer);
            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                { "username", username },
                { "customer_id", customer },
                { "sign", signature },
            };
            var json = JsonConvert.SerializeObject(parameters);
            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpointDev + "inquiry-pln", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<YourResponsePln>(responseContent);
                string rc = responseObject.data.rc;
                if (rc != "00")
                {
                    throw new CustomException(400, "Error", "data tidak ditemukan");
                }
                // var pricelist = awaitresponseObject.data.pricelist;
                return responseObject.data.name;
            }
            catch (CustomException)
            {
                throw;
            }
        }
        public async Task<Object> RefreshData()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var CreateSha = new CreateSha(configuration);
            string signature = CreateSha.md5Conv("pl");
            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                { "status", "all" },
                { "username", username },
                { "sign", signature },
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
                return new { code = 200, message = "Berhasil", data = transactionDataList };
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

        public class YourResponsePln
        {
            public ResponsePln data { get; set; }
        }

        public class CheckOPT
        {
            public string message { get; set; }
            public string rc { get; set; }

        }

        public class Data
        {
            public List<PricePrepaid> pricelist { get; set; }
        }

        public class ResponsePln
        {
            public string status { get; set; }
            public string customerId { get; set; }
            public string meterNo { get; set; }
            public string subscriberId { get; set; }
            public string name { get; set; }
            public string segmentPower { get; set; }
            public string message { get; set; }
            public string rc { get; set; }


        }
    }
}