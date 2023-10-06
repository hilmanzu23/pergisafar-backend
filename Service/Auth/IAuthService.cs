
using pergisafar.Shared.Models;
using pergisafar.Shared.Helper;
using Sieve.Models;
using static RepositoryPattern.Services.AuthService.AuthService;

public interface IAuthService
{
    Task<Object> LoginAsync(UserForm model);
    Task<Object> RegisterAsync(UserRegisterCustomer model);
}
