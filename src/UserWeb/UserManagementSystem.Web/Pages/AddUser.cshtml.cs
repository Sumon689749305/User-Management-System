using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementSystem.Web.Models;

namespace UserManagementSystem.Web.Pages
{
    public class AddUserModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public AddUserModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Models.AddUserModel User { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var token = HttpContext.Request.Cookies["jwtToken"];
            var client = _clientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Users", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/User");
            }

            ModelState.AddModelError(string.Empty, "Failed to add user");
            return Page();
        }
    }
}
