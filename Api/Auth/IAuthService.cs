
using static RepositoryPattern.Services.AuthService.AuthService;

public interface IAuthService
{
    Task<Object> LoginAsync(LoginDto model);
    Task<Object> RegisterAsync(RegisterDto model);
    Task<Object> Aktifasi(string id);
    Task<Object> UpdatePassword(string id, UpdatePasswordDto model);
    Task<Object> VerifyOtp(string id, OtpDto otp);
    Task<Object> RequestOtpEmail(string id);

}
