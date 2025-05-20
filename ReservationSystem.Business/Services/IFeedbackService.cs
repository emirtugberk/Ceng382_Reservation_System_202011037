using System.Threading.Tasks;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Business.Services
{
    public interface IFeedbackService
    {
        Task AddAsync(Feedback feedback);
        Task<int> GetUserIdByUsernameAsync(string username);
    }
}
