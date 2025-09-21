using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class RequireLoginMiddleware
{
    private readonly RequestDelegate _next;

    public RequireLoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        var isLoggedIn = context.Request.Cookies.ContainsKey("jwtToken");
  // Pages allowed without login
    var allowedPaths = new[]
    {
        "/deshboard",
        "/login",
        "/adduser",
        "/logout",
        "/css",
        "/js",
        "/lib",
        "Styles"
    };

    // If not logged in and path not in allowed list → redirect to login
    if (!isLoggedIn && !allowedPaths.Any(p => path.Contains(p)))
    {
        context.Response.Redirect("/Login");
        return;
    }

        await _next(context);
    }
}