using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MMN.Api.Helpers
{
    public class DebugBypassGuard
    {
        private static readonly string[] BlockedCookies  = { "oh_vida_oh_ceus" };
        private static readonly string[] BlockedHeaders  = { "X-Debug-Bypass", "X-Skip-Auth" };

        private readonly RequestDelegate _next;
        private readonly bool _isDevelopment;
        private readonly ILogger<DebugBypassGuard> _logger;

        public DebugBypassGuard(RequestDelegate next, IWebHostEnvironment env, ILogger<DebugBypassGuard> logger)
        {
            _next = next;
            _isDevelopment = env.IsDevelopment();
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            foreach (var name in BlockedCookies)
            {
                if (!context.Request.Cookies.ContainsKey(name)) continue;

                _logger.LogWarning("[DebugBypassGuard] debug cookie '{Name}' from {IP}", name, context.Connection.RemoteIpAddress);

                if (!_isDevelopment)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
            }

            foreach (var name in BlockedHeaders)
            {
                if (!context.Request.Headers.ContainsKey(name)) continue;

                _logger.LogWarning("[DebugBypassGuard] debug header '{Name}' from {IP}", name, context.Connection.RemoteIpAddress);

                if (_isDevelopment)
                    context.Request.Headers.Remove(name);
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class DebugBypassGuardExtensions
    {
        public static IApplicationBuilder UseDebugBypassGuard(this IApplicationBuilder app)
            => app.UseMiddleware<DebugBypassGuard>();
    }
}
