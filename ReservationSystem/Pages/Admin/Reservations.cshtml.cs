using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ReservationsModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public ReservationsModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IList<Reservation> PendingReservations { get; set; }
    public HashSet<int> ConflictIds { get; set; } = new();

    public async Task OnGetAsync()
    {
        PendingReservations = await _dbContext.Reservations
            .Where(r => r.Status == "Pending")
            .Include(r => r.User)
            .Include(r => r.Term)
            .ToListAsync();

        // Çakışma kontrolü
        for (int i = 0; i < PendingReservations.Count; i++)
        {
            var r1 = PendingReservations[i];
            for (int j = i + 1; j < PendingReservations.Count; j++)
            {
                var r2 = PendingReservations[j];
                if (r1.Classroom == r2.Classroom &&
                    r1.TermId == r2.TermId &&
                    r1.DayOfWeek == r2.DayOfWeek &&
                    r1.StartTime < r2.EndTime &&
                    r1.EndTime > r2.StartTime)
                {
                    ConflictIds.Add(r1.Id);
                    ConflictIds.Add(r2.Id);
                }
            }
        }
    }

    public async Task<IActionResult> OnPostApproveAsync(int id)
    {
        var reservation = await _dbContext.Reservations.FindAsync(id);
        if (reservation == null) return NotFound();
        reservation.Status = "Approved";
        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRejectAsync(int id)
    {
        var reservation = await _dbContext.Reservations.FindAsync(id);
        if (reservation == null) return NotFound();
        reservation.Status = "Rejected";
        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }
}
