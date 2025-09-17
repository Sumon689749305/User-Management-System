using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagementSystem.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser>? _signInManager;

        // inject SignInManager only if you use Identity; otherwise omit it
        public LogoutModel(SignInManager<IdentityUser>? signInManager = null)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Delete JWT cookie explicitly
            Response.Cookies.Delete("jwtToken");

            // Optionally clear any antiforgery cookies too
            Response.Cookies.Delete(".AspNetCore.Antiforgery.KowEZev8T1g");

            // Just in case you're mixing Identity
            await HttpContext.SignOutAsync();

            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await OnGetAsync();
        }
    }
}
