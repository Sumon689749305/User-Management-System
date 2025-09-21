using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

        [BindProperty]
        public SearchItem SearchModel { get; set; } = new();

        public List<UserDto> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var token = HttpContext.Request.Cookies["jwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            // Create the request object
            var request = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 0,
                Order = new List<Order> { new Order { Column = 0, Dir = "asc" } },
                Search = new Search { Value = "" },
                SearchItem = new SearchItem { Name = "", UserName = "" }
            };

            // Send POST request to API
            var response = await client.PostAsJsonAsync("api/Users/GetUsersData", request);
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<DataTableResponse<string[]>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null)
                {
                    // Map string[] to UserDto
                    Users = result.Data.Select(x => new UserDto
                    {
                        Name = x[0],
                        UserName = x[1],
                        Id = int.Parse(x[2])
                    }).ToList();

                }
            }
            else
            {
                Users = new List<UserDto>();
            }
        }

        public async Task OnPostAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var token = HttpContext.Request.Cookies["jwtToken"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestt = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 10,
                Order = new List<Order> { new Order { Column = 0, Dir = "asc" } },
                Search = new Search { Value = "" },
                SearchItem = new SearchItem
                {
                    Name = SearchModel?.Name ?? string.Empty,
                    UserName = SearchModel?.UserName ?? string.Empty
                }
            };
            // Send POST request to API
            var response = await client.PostAsJsonAsync("api/Users/GetUsersData", requestt);

            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<DataTableResponse<string[]>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null)
                {
                    // Map string[] to UserDto
                    Users = result.Data.Select(x => new UserDto
                    {
                        Name = x[0],
                        UserName = x[1],
                        Id = int.Parse(x[2])
                    }).ToList();

                }
            }
            else
            {
                Users = new List<UserDto>();
            }
        }
    }

    public class SearchRequest
    {
        public SearchItem SearchItem { get; set; } = new();
        public int Start { get; set; } = 0;
        public int Length { get; set; } = 10;
        public List<Order> Order { get; set; } = new() { new Order { Column = 1, Dir = "asc" } };
        public Search Search { get; set; } = new() { Regex = false, Value = "" };
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class DataTableRequest
    {
        public SearchItem SearchItem { get; set; } = new();
        public int Draw { get; set; } 
        public int Start { get; set; }
        public int Length { get; set; }
        public List<Order> Order { get; set; } = new();
        public Search Search { get; set; } = new();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class SearchItem
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; } = "asc";
    }

    public class Search
    {
        public bool Regex { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    public class UserResponse
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<UserDto> Data { get; set; } = new();
    }
    public class DataTableResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; } = new();
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}