using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using EShop.Core.Model.Enums;
using EShop.Core.Infrastructure.ActionResults;

namespace EShop.Core.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            JsonErrorResponse json = new()
            {
                ErrorCode = Guid.NewGuid().ToString().ToUpper()
            };

            if (context.Exception.GetType().Name == "ValidationException")
            {
                json.Messages = ((ValidationException)context.Exception).Errors.FirstOrDefault().ErrorMessage;
                json.StatusCode = ResponseStatus.ValidationError;
                json.DeveloperMessage = context.Exception.InnerException;
            }
            else
            {
                json.Messages = "An error occurred. Try it again.";
                json.StatusCode = ResponseStatus.InternalServerError;
                json.DeveloperMessage = context.Exception;

                _logger.LogCritical("Critical error condition. {@json}", json);
            }

            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.ExceptionHandled = true;

        }

        private class JsonErrorResponse
        {
            public ResponseStatus StatusCode { get; set; }
            public string Messages { get; set; }
            public object DeveloperMessage { get; set; }
            public string ErrorCode { get; set; }
        }
    }
}