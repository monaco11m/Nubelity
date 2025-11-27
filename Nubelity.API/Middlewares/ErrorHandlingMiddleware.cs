using Nubelity.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Nubelity.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            object errorResponse;

            switch (ex)
            {
                case NotFoundException notFound:
                    statusCode = HttpStatusCode.NotFound;
                    errorResponse = new { message = notFound.Message };
                    break;

                case DomainValidationException validation:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse = new { message = validation.Message, errors = validation.Errors };
                    break;

                case UnauthorizedException unauthorized:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorResponse = new { message = unauthorized.Message };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

                    if (isDevelopment)
                    {
                        errorResponse = new
                        {
                            message = "Development Show Error : " + ex.Message,
                            stackTrace = ex.StackTrace,
                            innerException = ex.InnerException?.Message
                        };
                    }
                    else
                    {
                        errorResponse = new { message = "An unexpected error occurred." };
                    }

                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
