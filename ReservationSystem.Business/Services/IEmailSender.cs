using System.Threading.Tasks;

namespace ReservationSystem.Business.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
