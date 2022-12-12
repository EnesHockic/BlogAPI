using BlogAPI.Core.Application.Common.Exceptions;
using FluentValidation.Results;
using System.Net;
using System.Text.Json;

namespace BlogAPI.Filters
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
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
                await HandleErrorAsync(context, ex);
            }
        }
        private static Task HandleErrorAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            var message = "";
            var stackTrace = "";
            var errorList = new List<ValidationFailure>();
            var exceptionType = ex.GetType();

            if(exceptionType == typeof(FluentValidation.ValidationException))
            {
                var exception = ex as FluentValidation.ValidationException;
                statusCode = HttpStatusCode.BadRequest;
                errorList = exception.Errors.ToList();
                message = "Validation error";
                stackTrace = exception.StackTrace;
            }
            else if(exceptionType == typeof(NotFoundException))
            {
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message;
                stackTrace = ex.StackTrace;
            }
            else if (exceptionType == typeof(DuplicateException))
            {
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                stackTrace = ex.StackTrace;
            }
            else
            {
                message = ex.Message;
                statusCode = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
            }
            var exceptionRes = JsonSerializer.Serialize(new { message = message, errors = errorList, stackTrace = stackTrace });
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(exceptionRes);
        }
    }
}
