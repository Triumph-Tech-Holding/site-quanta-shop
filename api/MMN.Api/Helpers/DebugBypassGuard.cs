using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MMN.Api.Helpers
{
    /// <summary>
    /// Middleware that removes/neutralises any known debug-bypass cookies or headers
    /// before the request reaches authentication and business logic.
    ///
    /// In Production/Staging: responds with 403 immediately when a blocked identifier
    /// is present so the request never reaches downstream handlers.
    /// In Development: strips the cookie/header and logs a warning so development
    /// workflows are not broken by stale browser cookies.
    ///
    /// Deny-listed identifiers:
    ///   Cookie  : oh_vida_oh_ceus
    ///   Header  : X-Debug-Bypass
    ///   Header  : X-Skip-Auth
    /// </summary>
    public class DebugBypassGuard
    {
        private static readonly string[] BlockedCookieNames = { "oh_vida_oh_ceus" };
        private static readonly string[] BlockedHeaderNames = { "X-Debug-Bypass", "X-Skip-Auth" };

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
            foreach (var cookieName in BlockedCookieNames)
            {
                if (context.Request.Cookies.ContainsKey(cookieName))
                {
                    if (_isDevelopment)
                    {
                        // Strip the cookie and continue — avoids breaking dev workflows with stale cookies
                        _logger.LogWarning(
                            "[DebugBypassGuard] Debug-bypass cookie '{Cookie}' detected and stripped in Development. " +
                            "Remove this cookie from your browser to eliminate this warning.", cookieName);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "[DebugBypassGuard] Blocked request with debug-bypass cookie '{Cookie}' from {IP}.",
                            cookieName, context.Connection.RemoteIpAddress);
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
                }
            }

            foreach (var headerName in BlockedHeaderNames)
            {
                if (context.Request.Headers.ContainsKey(headerName))
                {
                    if (_isDevelopment)
                    {
                        _logger.LogWarning(
                            "[DebugBypassGuard] Debug-bypass header '{Header}' detected and stripped in Development.",
                            headerName);
                        context.Request.Headers.Remove(headerName);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "[DebugBypassGuard] Blocked request with debug-bypass header '{Header}' from {IP}.",
                            headerName, context.Connection.RemoteIpAddress);
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }
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
