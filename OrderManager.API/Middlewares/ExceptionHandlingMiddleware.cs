using System.Net;
using System.Text.Json;

namespace OrderManager.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware or endpoint
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
        //    context.Response.ContentType = "application/json";

        //    var statusCode = exception switch
        //    {
        //        ValidationException => HttpStatusCode.BadRequest,
        //        UnauthorizedAccessException => HttpStatusCode.Unauthorized,
        //        KeyNotFoundException => HttpStatusCode.NotFound,
        //        OrderStateException => HttpStatusCode.Conflict, // Domain exception for state errors
        //        _ => HttpStatusCode.InternalServerError
        //    };

        //    context.Response.StatusCode = (int)statusCode;

        //    var response = new ErrorResponse
        //    {
        //        StatusCode = context.Response.StatusCode,
        //        Message = exception.Message,
        //        Details = (statusCode == HttpStatusCode.InternalServerError && _hostEnvironment.IsDevelopment())
        //            ? exception.StackTrace
        //            : null // Include details only for 500 and development environment
        //    };

        //    return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        //}
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized ,
                InvalidOperationException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
    {
        StatusCode = context.Response.StatusCode,
        Message = statusCode switch
        {
            HttpStatusCode.InternalServerError => _hostEnvironment.IsDevelopment() 
                ? exception.Message 
                : "An unexpected error occurred. Please try again later.",
            HttpStatusCode.Unauthorized => "You are not authorized to access this resource.",
            _ => exception.Message
        },
        Details = _hostEnvironment.IsDevelopment()  ? exception.StackTrace : null
    };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
