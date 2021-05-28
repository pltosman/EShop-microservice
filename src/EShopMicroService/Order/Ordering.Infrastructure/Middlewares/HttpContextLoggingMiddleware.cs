using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ordering.Infrastructure.Helpers;

namespace Ordering.Infrastructure.Middlewares
{
  public class HttpContextLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string requestBodyPayload = await ReadRequestBody(context.Request);
            LogHelper.RequestPayload = requestBodyPayload;

            var originalResponseBodyStream = context.Response.Body;

            using MemoryStream responseBody = new();
            context.Response.Body = responseBody;
            await _next(context);
            await responseBody.CopyToAsync(originalResponseBodyStream);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }
    }
}
