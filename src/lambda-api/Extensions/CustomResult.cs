using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

namespace lambda_api.Extensions
{
    public class CustomResult : IResult
    {
        private readonly string? _message;
        private readonly int _statusCode;
        private readonly object? _data;

        public CustomResult(int status, object? data)
        {
            _statusCode = status;
            _data = data;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = _statusCode;
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "*");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            await httpContext.Response.WriteAsJsonAsync(_data);
        }
    }
}
