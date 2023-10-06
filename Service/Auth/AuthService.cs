

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using pergisafar.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using MongoExample.Models;
using BCrypt.Net;

namespace RepositoryPattern.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<User> dataUser;

        public AuthService(IConfiguration configuration, IOptions<MongoDBSettings> mongoDBSettings)
        {
            _configuration = configuration;
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            dataUser = database.GetCollection<User>("users");
        }

        private string CreateJWT(UserForm? user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Mx0K7BAwAk+YXsKErRDtbTAeEpZEdHcSeKO15Snw/RaKd+Dnfb3XfX60F4AQHaG1")); // NOTE: SAME KEY AS USED IN Program.cs FILE
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[] // NOTE: could also use List<Claim> here
			{
                new Claim(ClaimTypes.Name, user.Email), // NOTE: this will be the "User.Identity.Name" value
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Email) // NOTE: this could a unique ID assigned to the user by a database
			};

            var token = new JwtSecurityToken(issuer: "pergisafar.com", audience: "pergisafar.com", claims: claims, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
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
                string token = CreateJWT(login);
                string idAsString = user.Id.ToString();
                return new { id = idAsString, accessToken = token };
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
                    Id = ObjectId.GenerateNewId(),
                    Email = data.Email,
                    Password = hashedPassword,
                    IsActive = true,
                    IsVerification = false,
                    Balance = 0,
                    Point = 0,
                    PhoneNumber = 0,
                    CreatedAt = DateTime.Now
                };

                await dataUser.InsertOneAsync(roleData);
                string roleIdAsString = roleData.Id.ToString();
                return new { message = "pendaftaran berhasil silahkan cek email untuk melakukan aktifasi", id = roleIdAsString };
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message };
            }
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
    }
}