﻿namespace HotelSystem.Common.Infrastructure
{
    using HotelSystem.Common.Services.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using static InfrastructureConstants;

    public class JwtHeaderAuthenticationMiddleware : IMiddleware
    {

        private readonly ICurrentTokenService currentToken;

        public JwtHeaderAuthenticationMiddleware(ICurrentTokenService currentToken)
            => this.currentToken = currentToken;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers[AuthorizationHeaderName].ToString();

            if (!string.IsNullOrWhiteSpace(token))
            {
                this.currentToken.Set(token.Split().Last());
            }

            await next.Invoke(context);
        }
    }

    public static class JwtHeaderAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtHeaderAuthentication(
            this IApplicationBuilder app)
            => app
                .UseMiddleware<JwtHeaderAuthenticationMiddleware>()
                .UseAuthentication();
    }
}
