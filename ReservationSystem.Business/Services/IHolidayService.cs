using System;
using System.Threading.Tasks;

namespace ReservationSystem.Business.Services
{
    public interface IHolidayService
    {
        Task<bool> IsHolidayAsync(DateTime date);

    }
}

