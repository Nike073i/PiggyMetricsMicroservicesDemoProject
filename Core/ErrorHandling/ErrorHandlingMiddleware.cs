using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Core.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="next">Делегат</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            object errorObjeсt = new object();

            switch (exception)
            {
                case ServiceException ex:
                    {
                        Log.Logger.Error("Ошибка: {errorMessage}", ex.Message);
                        errorObjeсt = ex.GetError();
                        code = HttpStatusCode.BadRequest;
                        break;
                    }
                default:
                    {
                        string absoluteURL = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                        Log.Logger.Error(exception, $"Произошла непредвиденная ошибка. Запрос: {absoluteURL}");
                        errorObjeсt = new ServiceError { Message = exception.Message, ErrorCode = ErrorCode.UnspecifiedError };
                        code = HttpStatusCode.InternalServerError;
                        break;
                    }
            }

            string result = JsonConvert.SerializeObject(errorObjeсt, Formatting.Indented);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
