using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Notifier.Frontend.ViewModel;
using System.Security.Claims;

namespace Notifier.Frontend.Pages.Account
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(loginViewModel.Username));
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginViewModel.Username)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/chat");
        }
    }
}
