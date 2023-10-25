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
        private readonly string apidev;
        private readonly string apiprod;
        private readonly string sign;
        private readonly string endpointDev;



        public PricePrepaidService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<PricePrepaid>("priceprepaid");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
            this.username = configuration.GetSection("IAKSettings")["Username"];
            this.apidev = configuration.GetSection("IAKSettings")["Dev"];
            this.apiprod = configuration.GetSection("IAKSettings")["Prod"];
            this.sign = configuration.GetSection("IAKSettings")["SecretKeyDevPL"];
            this.endpointDev = configuration.GetSection("IAKSettings")["EndPointDev"];
        }
        public async Task<Object> Get()
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
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpointDev + "pricelist", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<YourResponseType>(responseContent);
                List<ResponseDto> pricelist = responseObject.data.pricelist;
                return new { code = 200,Data = pricelist, message = "Data Add Complete" };
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
            public List<ResponseDto> pricelist { get; set; }
        }
    }
}