
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Infra.DTO;
using Shared.Infra.Exceptions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Shared.Infra.Middleware
{

    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
   

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var exceptionType = ex.GetType();
            HttpStatusCode status;
            object stackTrace;
            string message = ex.Message;

            if (ex is UnauthorizedAccessException)
            {
                status = HttpStatusCode.Unauthorized;
                stackTrace = "";
            }
            else if (ex is ExceptionBase exceptionBase)
            {
                if (exceptionBase is NotFoundException)
                    status = HttpStatusCode.NotFound;            
                else if (exceptionBase is PaymentRequiredException)
                    status = HttpStatusCode.PaymentRequired;
                else if (exceptionBase is BadRequestException)
                    status = HttpStatusCode.BadRequest;
                else
                    status = HttpStatusCode.InternalServerError;

                stackTrace = exceptionBase.ModelState.Count == 0 ? exceptionBase.ErrorString : exceptionBase.ModelState;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
                Log.Error(ex,ex.Message,ex.StackTrace);
            }

            var apiResponse = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Error = stackTrace
            };

            

            var exceptionResult = JsonSerializer.Serialize(apiResponse);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }

    }
}
