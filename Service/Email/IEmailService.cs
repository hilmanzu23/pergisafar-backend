
using SendingEmail;
using static RepositoryPattern.Services.AuthService.AuthService;

public interface IEmailService
{
    Task SendingEmail(EmailForm model);

}
