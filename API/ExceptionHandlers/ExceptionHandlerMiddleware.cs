using System.Net;
using API.ExceptionHandlers;
using Data.Enum;
using Newtonsoft.Json;

namespace API.Exception {
    public class ExceptionHandlerMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;


        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            }
            catch (AppException ex) {
                _logger.LogError(ex, "an error occured processing the request");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, AppException ex) {
            var statusCode = MapStatusCode(ex.ErrorCode);
            var response = new {
                message = ex.Message,
                errorType = ex.ErrorCode.ToString(),
                statusCode = statusCode
            };

            var jsonResponse = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(jsonResponse);
        }

        private static int MapStatusCode(ErrorCode errorCode) {
            return errorCode switch {
                ErrorCode.ResourceNotFound => (int)HttpStatusCode.NotFound,
                ErrorCode.AuthenticationError => (int)HttpStatusCode.Unauthorized,
                // Map other error codes...
                _ => (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
