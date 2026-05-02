using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Helpers
{
    /// <summary>
    /// Middleware that explicitly blocks any known debug-bypass cookies or headers
    /// that could allow unauthorized access to internal error details or route
    /// authentication. Active in all environments — there is no debug bypass in
    /// this application at any environment level.
    ///
    /// Known blocked identifiers (deny-list):
    ///   - Cookie: oh_vida_oh_ceus
    ///   - Header: X-Debug-Bypass
    ///   - Header: X-Skip-Auth
    /// </summary>
    public class DebugBypassGuard
    {
        private static readonly string[] BlockedCookieNames =
        {
            "oh_vida_oh_ceus"
        };

        private static readonly string[] BlockedHeaderNames =
        {
            "X-Debug-Bypass",
            "X-Skip-Auth"
        };

        private readonly RequestDelegate _next;

        public DebugBypassGuard(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            foreach (var cookieName in BlockedCookieNames)
            {
                if (context.Request.Cookies.ContainsKey(cookieName))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
            }

            foreach (var headerName in BlockedHeaderNames)
            {
                if (context.Request.Headers.ContainsKey(headerName))
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
