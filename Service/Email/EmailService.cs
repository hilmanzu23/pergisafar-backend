using System.Net;
using System.Net.Mail;
using RepositoryPattern.Services.AuthService;

namespace SendingEmail
{
    public class EmailService : IEmailService
    {
        public Task SendingEmail(EmailForm model)
        {
            var mail = "travelberkah23@outlook.com";
            var pw = "padang123";

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(new MailMessage(from: mail,to: model.Email,model.Subject,model.Message));
        }
    }

    public class EmailForm
    {
        public string? Email {get; set;}
        public string? Subject {get; set;}
        public string? Message {get; set;}

    }
}