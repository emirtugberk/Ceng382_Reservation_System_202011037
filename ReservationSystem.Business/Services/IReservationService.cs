using ReservationSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationSystem.Business.Services
{
    public interface IReservationService
    {
        /// <summary>
        /// Yeni bir rezervasyon oluşturur.
        /// </summary>
        Task CreateReservationAsync(Reservation reservation);

        /// <summary>
        /// Belirtilen kullanıcıya ait tüm rezervasyonları getirir.
        /// </summary>
        Task<List<Reservation>> GetByUserAsync(int userId);

        /// <summary>
        /// Statüsü "Pending" olan tüm rezervasyonları getirir.
        /// </summary>
        Task<List<Reservation>> GetPendingReservationsAsync();
    }
}
