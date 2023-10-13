using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using pergisafar.Shared.Models;
using Newtonsoft.Json;
using CheckId;

namespace RepositoryPattern.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<Payment> dataUser;
        private readonly IMongoCollection<User> users;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly string key;

        public PaymentService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Payment>("transactions");
            users = database.GetCollection<User>("users");
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
        }

        public async Task<object> GetPayment()
        {
            string amount = "10000";
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string merchantCode = _configuration["MerchantCode"];
            string secretKey = _configuration["SecretKey"];

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

                    return new { data = jsonObject, success = false };
                }
                else
                {
                    // Handle the error response here.
                    return new { data = "Error", success = false };
                }
            }
        }

        public async Task<Object> MakePayment(CreatePaymentDto createPaymentDto)
        {
            string merchantCode = _configuration["MerchantCode"];
            string merchantOrder = Guid.NewGuid().ToString();
            string secretKey = _configuration["SecretKey"];
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
                    email = createPaymentDto.Email,
                    phoneNumber = createPaymentDto.PhoneNumber,
                    itemDetails = new[]
                    {
                        new
                        {
                            name = createPaymentDto.FullName,
                            price = createPaymentDto.PaymentAmount.ToString(),
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
                    var roleData = new Payment()
                    {
                        Id = merchantOrder,
                        IdUser = createPaymentDto.IdUser,
                        IdTransactions = Transaksi.TopUp,
                        Signature = hash,
                        IsActive = true,
                        IsVerification = false,
                        CreatedAt = DateTime.Now,
                        Data = jsonObject,
                        Amount = createPaymentDto.PaymentAmount
                    };
                    await dataUser.InsertOneAsync(roleData);
                    return new { success = true, data = jsonObject };
                }
                else
                {
                    // Handle the error response here.
                    return new { success = false };
                }
            }
        }

        public async Task<Object> GetId(string id)
        {
            var items = await dataUser.Find(_ => _.Id == id).FirstOrDefaultAsync();

            if (items == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            return new
            {
                success = true,
                data = items
            };
        }

        public async Task<object> ApprovalPayment(string merchantOrderId)
        {
            try
            {
                var check = await dataUser.Find(x => x.Id == merchantOrderId).FirstOrDefaultAsync();

                if (check == null)
                {
                    return new { success = false, message = "Invalid request." };
                }

                if (check.IsVerification == true)
                {
                    return new { success = false, message = "Invalid request." };
                }

                if (check.IdTransactions == Transaksi.TopUp)
                {
                    var checkUser = await users.Find(x => x.Id == check.IdUser).FirstOrDefaultAsync();
                    if (checkUser == null)
                    {
                        return new { success = false, message = "User not found." };
                    }
                    //update saldo
                    checkUser.Balance = checkUser.Balance + check.Amount;
                    await users.ReplaceOneAsync(x => x.Id == check.IdUser, checkUser);
                    //update transaksi
                    check.IsVerification = true;
                    await dataUser.ReplaceOneAsync(x => x.Id == merchantOrderId, check);
                    return new { success = true, data = check };
                }

                return new { success = true, data = check };
            }
            catch (Exception ex)
            {
                return new { success = false, message = "An error occurred.", error = ex.Message };
            }
        }
        // 50dc73b7-50c3-4564-9f55-6f866609f840
        public class CreatePaymentDto
        {
            public string PaymentMethod { get; set; }
            public string PhoneNumber { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public double PaymentAmount { get; set; }
            public string ProductDetails { get; set; }
            public string IdUser { get; set; }
        }
    }
}