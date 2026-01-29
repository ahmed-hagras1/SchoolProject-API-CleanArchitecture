using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SchoolProject.Core.Bases;
using System.Net;
using System.Text.Json;

namespace SchoolProject.API.MiddleWares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                // 1. Get the Status Code based on the Exception Type
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        responseModel.StatusCode = HttpStatusCode.Unauthorized;
                        break;

                    case ValidationException:
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
                        break;

                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.StatusCode = HttpStatusCode.NotFound;
                        break;

                    case DbUpdateException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        break;

                    default:
                        // Internal Server Error for everything else
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.StatusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                // 2. Serialize using standard Web Options (camelCase)
                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await response.WriteAsync(result);
            }
        }
    }
}