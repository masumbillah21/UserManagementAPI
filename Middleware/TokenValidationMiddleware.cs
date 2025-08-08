using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using UserManagementAPI.Responses;

namespace UserManagementAPI.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string AUTH_HEADER = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private const string VALID_TOKEN = "secret";

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/users"))
            {
                if (!context.Request.Headers.ContainsKey(AUTH_HEADER))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    var response = System.Text.Json.JsonSerializer.Serialize(new ApiResponse {
                        Success = false,
                        Message = "Unauthorized: Missing token.",
                        Data = null
                    });
                    await context.Response.WriteAsync(response);
                    return;
                }

                var token = context.Request.Headers[AUTH_HEADER].ToString();
                if (token.StartsWith(BEARER_PREFIX))
                    token = token.Substring(BEARER_PREFIX.Length);

                if (token != VALID_TOKEN)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    var response = System.Text.Json.JsonSerializer.Serialize(new ApiResponse {
                        Success = false,
                        Message = "Unauthorized: Invalid token.",
                        Data = null
                    });
                    await context.Response.WriteAsync(response);
                    return;
                }
            }
            await _next(context);
        }
    }
}
