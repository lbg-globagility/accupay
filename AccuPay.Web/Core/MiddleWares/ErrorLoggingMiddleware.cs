using AccuPay.Data.Exceptions;
using AccuPay.Infrastructure.Services.Excel;
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
            catch (WorkSheetRowParseValueException ex)
            {
                await HandleDomainExceptions(context, ex.Message);
            }
            catch (ExcelException ex)
            {
                await HandleDomainExceptions(context, ex.Message);
            }
            catch (BusinessLogicException ex)
            {
                await HandleDomainExceptions(context, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Uncaught Exception");
                throw;
            }
        }

        private Task HandleDomainExceptions(HttpContext context, string message)
        {
            var result = JsonConvert.SerializeObject(new { Error = message });
            context.Response.ContentType = CONTENT_TYPE;
            context.Response.StatusCode = (int)400;

            return context.Response.WriteAsync(result);
        }
    }
}
