using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Task7.WebApplication.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Session.GetString("auth");
            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            await _next.Invoke(context);

            //var token = context.Request.Cookies[".AspNetCore.Application.Id"];
            //if (!string.IsNullOrEmpty(token))
            //    context.Request.Headers.Add("Authorization", "Bearer " + token);
        }
    }
}
