
using static RepositoryPattern.Services.AuthService.AuthService;

public interface IAuthService
{
    Task<Object> LoginAsync(UserForm model);
    Task<Object> RegisterAsync(UserRegisterCustomer model);
    Task<Object> Aktifasi(string id);
    Task<Object> UpdatePassword(string id, UpdatePasswordForm model);
    Task<Object> VerifyOtp(string id, OtpForm otp);
    Task<Object> RequestOtpEmail(string id);

}
