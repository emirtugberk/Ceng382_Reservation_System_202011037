using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHasher<User> _hasher;

        public LoginModel(ApplicationDbContext dbContext, IPasswordHasher<User> hasher)
        {
            _dbContext = dbContext;
            _hasher    = hasher;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = "/")
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
        {
            if (!ModelState.IsValid)
                return Page();

            // 1) Kullanıcıyı bul
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Username == Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                return Page();
            }

            // 2) Şifre kontrolü
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, Password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                return Page();
            }

            // 3) Claim’leri oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 4) Cookie’ye yaz
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return LocalRedirect(returnUrl);
        }
    }
}
