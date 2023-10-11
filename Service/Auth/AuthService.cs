

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using pergisafar.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text;
using SendingEmail;

namespace RepositoryPattern.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> dataUser;
        private readonly IEmailService _emailService;
        private readonly string key;

        public AuthService(IConfiguration configuration, IEmailService emailService)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<User>("users");
            // var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
            _emailService = emailService;
        }

        public string Authenticate(UserForm login, string? id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keys = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Email, login.Email),
                    new(ClaimTypes.NameIdentifier, id)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keys), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "pergisafar.com",
                Audience = "pergisafar.com",
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<Object> LoginAsync([FromBody] UserForm login)
        {
            try
            {
                if (!IsValidEmail(login.Email))
                {
                    throw new Exception("Format Email salah.");
                }
                if (login.Password.Length < 8)
                {
                    throw new Exception("Password harus 8 karakter");
                }
                var filter = Builders<User>.Filter.Eq(u => u.Email, login.Email);
                var user = await dataUser.Find(filter).SingleOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception("Email atau Password Salah");
                }
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
                if (!isPasswordCorrect)
                {
                    throw new Exception("Email atau Password Salah");
                }
                if (user.IsActive == false)
                {
                    throw new Exception("Akun anda tidak perbolehkan akses");
                }
                if (user.IsVerification == false)
                {
                    throw new Exception("Akun anda belum aktif, silahkan aktifasi melalui link kami kirimkan di email anda");
                }
                string token = Authenticate(login, user.Id);
                string idAsString = user.Id.ToString();
                return new { success = true, id = idAsString, accessToken = token };
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message };
            }
        }

        public async Task<object> RegisterAsync([FromBody] UserRegisterCustomer data)
        {
            try
            {
                if (!IsValidEmail(data.Email))
                {
                    throw new Exception("Format Email salah.");
                }
                if (data.Password.Length < 8)
                {
                    throw new Exception("Password harus 8 karakter");
                }
                var filter = Builders<User>.Filter.Eq(u => u.Email, data.Email);
                var user = await dataUser.Find(filter).SingleOrDefaultAsync();

                if (user != null)
                {
                    throw new Exception("Email sudah digunakan.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(data.Password);

                var roleData = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = data.Email,
                    Password = hashedPassword,
                    IsActive = true,
                    IsVerification = false,
                    Balance = 0,
                    Point = 0,
                    PhoneNumber = "",
                    Pin = "",
                    IdRole = "ebe6b5b1-71f5-4872-b0ca-408184372153",
                    CreatedAt = DateTime.Now
                };

                await dataUser.InsertOneAsync(roleData);
                string roleIdAsString = roleData.Id.ToString();

                var email = new EmailForm()
                {
                    Email = data.Email,
                    Subject = "Aktifasi Travel Berkah",
                    Message = $"Pendaftaran Berhasil silahkan klik link ini untuk verifikasi https://localhost:7083/auth/aktifasi/{roleIdAsString}"
                };
                var sending = _emailService.SendingEmail(email);
                return new
                {
                    success = true,
                    message = "pendaftaran berhasil silahkan cek email untuk melakukan aktifasi",
                    id = roleIdAsString
                };
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message };
            }
        }

        public async Task<object> UpdatePassword(string id, UpdatePasswordForm item)
        {
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            if (item.Password.Length < 8)
            {
                throw new Exception("Password harus 8 karakter");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(item.Password);
            roleData.Password = hashedPassword;
            await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
            return new { success = true, id = roleData.Id.ToString() };
        }

        public async Task<object> RequestOtpEmail(string id)
        {
            var roleData = await dataUser.Find(x => x.Email == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            Random random = new Random();
            string otp = random.Next(10000, 99999).ToString();
            var email = new EmailForm()
            {
                Email = roleData.Email,
                Subject = "Request OTP",
                Message = $"Berikut adalah OTP anda /br {otp}"
            };
            var sending = _emailService.SendingEmail(email);
            roleData.Otp = otp;
            await dataUser.ReplaceOneAsync(x => x.Email == id, roleData);
            return new { success = true, id = roleData.Id.ToString() };
        }

        public async Task<object> VerifyOtp(string id, OtpForm otp)
        {
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            if (roleData.Otp != otp.Otp)
            {
                return new { success = false, errorMessage = "Wrong Otp" };
            }
            var data = new UserForm();
            {
                data.Email = roleData.Email;
            }
            string token = Authenticate(data, roleData.Id);
            return new { success = true, id = roleData.Id.ToString(), accessToken = token };
        }

        public class UserForm
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class UserRegisterCustomer
        {
            public string? Email { get; set; }
            public string? FullName { get; set; }
            public string? Password { get; set; }
        }

        public class UpdatePasswordForm
        {
            public string? Password { get; set; }
            public string? ConfirmPassword { get; set; }

        }

        public class OtpForm
        {
            public string? Otp { get; set; }

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<object> Aktifasi(string id)
        {
            var roleData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (roleData == null)
            {
                return new { success = false, errorMessage = "Data not found" };
            }
            roleData.IsVerification = true;
            await dataUser.ReplaceOneAsync(x => x.Id == id, roleData);
            return new { success = true, id = roleData.Id.ToString() };
        }
    }
}