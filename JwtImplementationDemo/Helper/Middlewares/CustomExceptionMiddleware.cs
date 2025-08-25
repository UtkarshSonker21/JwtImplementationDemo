using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Helper.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JwtImplementationDemo.Helper.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // log exception here
                _logger.LogError(ex.Message,ex.InnerException);

                await HandleExceptionAsync(context, ex);
            }
        }



        private static Task HandleExceptionAsync(HttpContext context , Exception ex)
        {
            var errorResponse = new ErrorModel();

            if (ex is BadRequestException badRequest)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                errorResponse.Message = badRequest.Message;
                errorResponse.ErrorCode = "BAD_REQUEST";
            }
            else if (ex is UnauthorizedException unauthorized)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                errorResponse.Message = unauthorized.Message;
                errorResponse.ErrorCode = "UNAUTHORIZED";
            }
            else if (ex is ForbiddenException forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                errorResponse.Message = forbidden.Message;
                errorResponse.ErrorCode = "FORBIDDEN";
            }
            else if (ex is NotFoundException notFound)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                errorResponse.Message = notFound.Message;
                errorResponse.ErrorCode = "NOT_FOUND";
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponse.Message = "Something went wrong.";
                errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";
            }

            return context.Response.WriteAsJsonAsync(errorResponse);
        }

    }
}
