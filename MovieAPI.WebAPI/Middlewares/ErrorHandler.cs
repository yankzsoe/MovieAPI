using System.Net;
using System.Text.Json;
using MovieAPI.Application.Common.Exceptions;
using MovieAPI.Application.Common.Models.Responses;

namespace MovieAPI.WebAPI.Middlewares {
    public class ErrorHandler {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next
            , ILogger<ErrorHandler> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception error) {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error.Message };

                switch (error) {
                    case ForbiddenException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        responseModel.Errors = new List<string>() { e.Error };
                        break;

                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Errors = new List<string>() { e.Error };
                        break;

                    case UnauthorizedException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        responseModel.Errors = new List<string>() { e.Error };
                        break;

                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "An error occurred while processing your request.";
                        responseModel.Errors = new List<string>() { error.Message };

                        //  only log internal server error
                        _logger.LogError("Unhandled exception: {errorMessage}", error.Message);

                        break;
                }

                var result = JsonSerializer.Serialize(responseModel
                    , new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                await response.WriteAsync(result);
            }
        }
    }
}
