using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Business.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext _db;
        public FeedbackService(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(Feedback feedback)
        {
            _db.Feedbacks.Add(feedback);
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetUserIdByUsernameAsync(string username)
        {
            var user = await _db.Users
                .SingleOrDefaultAsync(u => u.Username == username);
            if (user is null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");
            return user.Id;
        }
    }
}
