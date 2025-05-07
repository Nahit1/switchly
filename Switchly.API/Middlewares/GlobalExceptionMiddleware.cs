using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Switchly.Application.Common.Models;

namespace Switchly.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // bir sonraki middleware'e geç
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ApiResponse<object> errorResponse = ex switch
            {
                ValidationException validationEx =>
                    ApiResponse<object>.Fail(validationEx.Errors.Select(e => e.ErrorMessage).ToList()),

                _ => ApiResponse<object>.Fail("Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.")
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
        }
    }
}
