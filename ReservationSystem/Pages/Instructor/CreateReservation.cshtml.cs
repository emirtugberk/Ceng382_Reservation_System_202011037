using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Business.Services;
using ReservationSystem.Data;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Pages.Instructor
{
    public class CreateReservationModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly ApplicationDbContext    _dbContext;

        public List<SelectListItem> Terms       { get; set; }
        public List<SelectListItem> Classrooms  { get; set; }
        public List<SelectListItem> DaysOfWeek  { get; set; }
        public List<SelectListItem> StartTimes  { get; set; }
        public List<SelectListItem> EndTimes    { get; set; }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public CreateReservationModel(
            IReservationService reservationService,
            ApplicationDbContext dbContext)
        {
            _reservationService = reservationService;
            _dbContext          = dbContext;
        }

        public void OnGet()
        {
            // load terms from DB (Text=code, Value=ID)
            Terms = _dbContext.Terms
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem {
                    Text  = t.Name,
                    Value = t.Id.ToString()
                })
                .ToList();

            // classrooms 101–500
            Classrooms = Enumerable.Range(101, 400)
                .Select(i => new SelectListItem {
                    Text  = i.ToString(),
                    Value = i.ToString()
                })
                .ToList();

            // days of week
            DaysOfWeek = Enum.GetValues<DayOfWeek>()
                .Select(d => new SelectListItem {
                    Text  = d.ToString(),
                    Value = d.ToString()
                })
                .ToList();

            // start times xx:20
            StartTimes = Enumerable.Range(8, 11)
                .Select(h => new SelectListItem {
                    Text  = $"{h:D2}:20",
                    Value = $"{h:D2}:20"
                })
                .ToList();

            // end times xx:00
            EndTimes = Enumerable.Range(9, 11)
                .Select(h => new SelectListItem {
                    Text  = $"{h:D2}:00",
                    Value = $"{h:D2}:00"
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // *strip out* the two navigation props so they
            // won't trigger their [Required] on the model
            ModelState.Remove("Reservation.User");
            ModelState.Remove("Reservation.Term");

            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            // set the FK for the logged-in user
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Reservation.UserId = userId;

            await _reservationService.CreateReservationAsync(Reservation);
            return RedirectToPage("MyReservations");
        }
    }
}
