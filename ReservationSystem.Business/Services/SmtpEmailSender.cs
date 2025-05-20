using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ReservationSystem.Business.Services;

namespace ReservationSystem.Business.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;

        // IConfiguration yerine Options pattern kullanıyoruz
        public SmtpEmailSender(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new System.Net.NetworkCredential(_settings.User, _settings.Pass),
                EnableSsl = true
            };

            var mail = new MailMessage(_settings.User, to, subject, body);
            await client.SendMailAsync(mail);
        }
    }
}
