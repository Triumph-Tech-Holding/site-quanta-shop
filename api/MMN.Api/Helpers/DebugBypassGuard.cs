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
                        // In Development: log a warning and continue — request cookies are immutable
                        // so we cannot remove them, but we do not block. Developers should clear
                        // the stale cookie from their browser to eliminate this warning.
                        _logger.LogWarning(
                            "[DebugBypassGuard] Debug-bypass cookie '{Cookie}' detected in Development. " +
                            "Clear this cookie from your browser to suppress this warning.", cookieName);
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
                        // In Development: remove from the mutable request headers collection and log.
                        // This prevents downstream handlers from acting on the header without blocking
                        // the request, which avoids disrupting development workflows.
                        context.Request.Headers.Remove(headerName);
                        _logger.LogWarning(
                            "[DebugBypassGuard] Debug-bypass header '{Header}' removed in Development.",
                            headerName);
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
