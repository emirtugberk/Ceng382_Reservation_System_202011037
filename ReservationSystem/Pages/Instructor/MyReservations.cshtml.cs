using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Data.Entities;
using ReservationSystem.Business.Services;
using Microsoft.AspNetCore.Http;

namespace ReservationSystem.Pages.Instructor
{
    public class MyReservationsModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyReservationsModel(
            IReservationService reservationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _reservationService = reservationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Reservation> Reservations { get; set; } = Enumerable.Empty<Reservation>();

        public async Task OnGetAsync()
        {
            // Şu anki kullanıcı kimliğini al
            var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? _httpContextAccessor.HttpContext.User.FindFirstValue("sub");
            if (int.TryParse(userIdString, out var userId))
            {
                Reservations = await _reservationService.GetByUserAsync(userId);
            }
            else
            {
                Reservations = new List<Reservation>();
            }
        }
    }
}
