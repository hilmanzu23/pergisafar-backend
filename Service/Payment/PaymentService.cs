using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using pergisafar.Shared.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace RepositoryPattern.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<Payment> dataUser;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly string key;

        public PaymentService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Payment>("payment");
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }

        public async Task<object> GetPayment()
        {
            string merchantCode = "DS16190";
            string amount = "10000";
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string secretKey = "d45f0ff05e0147b7eb7053b0aa1a0224";

            string data = merchantCode + amount + dateTime + secretKey;
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(dataBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                string apiUrl = "https://sandbox.duitku.com/webapi/api/merchant/paymentmethod/getpaymentmethod";
                
                var requestData = new
                {
                    merchantcode = merchantCode,
                    amount = amount,
                    datetime = dateTime,
                    signature = hash
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<PaymentMethod.Temperatures>(responseContent);
                    return new {data = jsonObject , success = false};
                }
                else
                {
                    // Handle the error response here.
                    return new {data = "Error" , success = false};
                }
            }
        }

        public async Task<Object> MakePayment(CreatePaymentDto createPaymentDto)
        {
            string merchantCode = "DS16190";
            string merchantOrder = Guid.NewGuid().ToString();
            string secretKey = "d45f0ff05e0147b7eb7053b0aa1a0224";

            string data = merchantCode + merchantOrder + createPaymentDto.PaymentAmount + secretKey;
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(dataBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                string apiUrl = "https://sandbox.duitku.com/webapi/api/merchant/v2/inquiry";

                var requestData = new
                {
                    merchantCode = merchantCode,
                    paymentAmount = createPaymentDto.PaymentAmount,
                    paymentMethod = createPaymentDto.PaymentMethod,
                    merchantOrderId = merchantOrder,
                    productDetails = createPaymentDto.ProductDetails,
                    additionalParam = "",
                    merchantUserInfo = "",
                    customerVaName = createPaymentDto.FullName,
                    email = "hil@gmail.com",
                    phoneNumber = createPaymentDto.PhoneNumber,
                    itemDetails = new[]
                    {
                        new
                        {
                            name = createPaymentDto.FullName,
                            price = createPaymentDto.PaymentAmount.ToString(),///sumber masalah
                            quantity = 1
                        }
                    },
                    callbackUrl = _configuration["CallbackUrl"],
                    returnUrl = _configuration["ReturnUrl"],
                    signature = hash,
                    expiryPeriod = 15
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<PaymentMaking.Temperatures>(responseContent);
                    return new {success = true, data = jsonObject};
                }
                else
                {
                    // Handle the error response here.
                    return new {success = false};
                }
            }
        }

        private string GenerateUUID()
        {
            return Guid.NewGuid().ToString();
        }

        public class CreatePaymentDto
        {
            public string PaymentMethod { get; set; }
            public string PhoneNumber { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public double PaymentAmount { get; set; }
            public string ProductDetails { get; set; }
        }
    }
}