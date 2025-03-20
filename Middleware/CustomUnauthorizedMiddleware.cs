using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CustomUnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public CustomUnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context); // Ensure request is processed first

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            if (!context.Response.HasStarted) // ✅ Check if response has started
            {
                context.Response.ContentType = "application/json";
                var response = new { message = "Access Denied! You are not authorized to perform this action." };
                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
