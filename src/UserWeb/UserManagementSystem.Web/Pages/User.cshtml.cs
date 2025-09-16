using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagementSystem.Web.Pages
{
    public class UserModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public UserModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public List<UserDto> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Get JWT token from cookie
            var token = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
                return; // not logged in

            var client = _clientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Users = JsonSerializer.Deserialize<List<UserDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
        }

    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}