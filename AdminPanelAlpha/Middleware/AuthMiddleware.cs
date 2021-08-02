using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Game.AdminPanelAlpha.Middleware
{
    public class AuthMiddleware
    {

        private readonly RequestDelegate _next;
        private HttpClient _httpClient;
        public AuthMiddleware(RequestDelegate next, HttpClient httpClient)
        {
            _next = next;
            _httpClient = httpClient;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(
                    "Bearer", 
                    httpContext.Request.Cookies[".AspNetCore.Application.Token"]
                    );

            await _next(httpContext);
        }
    }
}
