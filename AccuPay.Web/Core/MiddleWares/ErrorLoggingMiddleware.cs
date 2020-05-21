using AccuPay.Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private const string CONTENT_TYPE = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("Serious Errors");
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessLogicException ex)
            {
                await HandleDomainExceptions(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Uncaught Exception");
                throw;
            }
        }

        private Task HandleDomainExceptions(HttpContext context, BusinessLogicException ex)
        {
            var result = JsonConvert.SerializeObject(new { Error = ex.Message });
            context.Response.ContentType = CONTENT_TYPE;
            context.Response.StatusCode = (int)400;

            return context.Response.WriteAsync(result);
        }
    }
}
