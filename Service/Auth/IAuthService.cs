
using static RepositoryPattern.Services.AuthService.AuthService;

public interface IAuthService
{
    Task<Object> LoginAsync(UserForm model);
    Task<Object> RegisterAsync(UserRegisterCustomer model);
    Task<Object> Aktifasi(string id);

}
