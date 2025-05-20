// ReservationSystem.Business/Services/ReservationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Business.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHolidayService _holidayService;

        public ReservationService(
            ApplicationDbContext dbContext,
            IHolidayService holidayService)
        {
            _dbContext = dbContext;
            _holidayService = holidayService;
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            // 1) Dönem kontrolü
            var term = await _dbContext.Terms.FindAsync(reservation.TermId);
            if (term == null)
                throw new ArgumentException("Geçersiz dönem ID’si");

            // 2) Tarihleri oluştur ve tatil kontrolü
            var dates = new List<DateTime>();
            for (var d = term.StartDate; d <= term.EndDate; d = d.AddDays(1))
            {
                if (d.DayOfWeek == reservation.DayOfWeek)
                    dates.Add(d);
            }

            reservation.IsHolidayWarning = false;
            foreach (var day in dates)
            {
                if (await _holidayService.IsHolidayAsync(day))
                {
                    reservation.IsHolidayWarning = true;
                    break;
                }
            }

            // 3) Zaman çakışması kontrolü
            var conflicts = await _dbContext.Reservations
                .Where(r =>
                    r.TermId == reservation.TermId &&
                    r.Classroom == reservation.Classroom &&
                    r.DayOfWeek == reservation.DayOfWeek &&
                    reservation.StartTime < r.EndTime &&
                    reservation.EndTime > r.StartTime)
                .ToListAsync();
            if (conflicts.Any())
                throw new InvalidOperationException("Seçilen zamanda çakışma var.");

            // 4) Kaydet + log
            reservation.Status = "Pending";
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();

            _dbContext.LogEntries.Add(new LogEntry
            {
                Action    = "Created reservation",
                Details   = $"ID:{reservation.Id}, Classroom:{reservation.Classroom}, Dates:{string.Join(',', dates)}",
                Timestamp = DateTime.UtcNow,
                UserId    = reservation.UserId
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetByUserAsync(int userId)
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.Term)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetPendingReservationsAsync()
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.Term)
                .Where(r => r.Status == "Pending")
                .ToListAsync();
        }
    }
}
