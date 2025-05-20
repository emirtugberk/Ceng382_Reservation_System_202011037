using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Business.Services;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Pages.Instructor
{
    public class FeedbackModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IFeedbackService _feedbackService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackModel(
            IReservationService reservationService,
            IFeedbackService feedbackService,
            IHttpContextAccessor httpContextAccessor)
        {
            _reservationService  = reservationService;
            _feedbackService     = feedbackService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<SelectListItem> ReservationList { get; set; }
        [BindProperty] public Feedback Feedback { get; set; }

        public async Task OnGetAsync()
        {
            // 1) Kullanıcı adını alıp Id’yi çekiyoruz
            var username = _httpContextAccessor.HttpContext!.User.Identity!.Name!;
            var userId   = await _feedbackService.GetUserIdByUsernameAsync(username);

            // 2) Servisten tüm rezervasyonları alıyoruz
            var allRes = await _reservationService.GetByUserAsync(userId);

            // 3) Sadece onaylanmış (Approved) olanları filtreleyip listeye dönüştürüyoruz
            var approved = allRes.Where(r => r.Status == "Approved");
            ReservationList = approved
                .Select(r => new SelectListItem(
                    $"{r.Classroom} / {r.DayOfWeek} {r.StartTime:hh\\:mm}–{r.EndTime:hh\\:mm}",
                    r.Id.ToString()))
                .ToList();

            // 4) Yeni Feedback objesini başlatıyoruz
            Feedback = new Feedback
            {
                CreatedAt = DateTime.UtcNow,
                UserId    = userId
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _feedbackService.AddAsync(Feedback);
            TempData["Message"] = "Geri bildiriminiz kaydedildi!";
            return RedirectToPage();
        }
    }
}
