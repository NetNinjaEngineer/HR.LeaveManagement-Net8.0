using HR.LeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace HR.LeaveManagement.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode = ex switch
        {
            BadRequestException badRequestException => HttpStatusCode.BadRequest,
            NotFoundException NotFound => HttpStatusCode.NotFound,
            ValidationException ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        ProblemDetails problemDetails = new()
        {
            Status = (int)statusCode,
            Title = ex.Message,
            Type = ex.GetType().ToString(),
            Detail = "Error Occurred"
        };

        var result = JsonSerializer.Serialize(problemDetails);

        _logger.LogError(result);

        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(result);
    }
}
