using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagementSystem.Web.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public DeleteModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public UserDto User { get; set; } = new();

        public async Task OnGetAsync(int id)
        {

            var token = HttpContext.Request.Cookies["jwtToken"];
            var client = _clientFactory.CreateClient("API");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/Users/id?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<UserDto>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var token = HttpContext.Request.Cookies["jwtToken"];
            var client = _clientFactory.CreateClient("API");

           
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/Users/id?id={User.Id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/User");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete user");
            return Page();
        }
        public class UserDto
        {
            public int Id { get; set; }
            public string UserName { get; set; }
        }
    }
}
