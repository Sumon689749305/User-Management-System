using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagementSystem.Web.Pages
{
    public class EditUserModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public EditUserModel(IHttpClientFactory clientFactory)
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

            var response = await client.GetAsync($"api/Users/GetUserById/id?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<UserDto>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");

            var token = HttpContext.Request.Cookies["jwtToken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(User), Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/Users/UpdateUser/id?id={id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/User");
            }

            ModelState.AddModelError(string.Empty, "Failed to update user");
            return Page();
        }
        public class UserDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string UserName { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}
