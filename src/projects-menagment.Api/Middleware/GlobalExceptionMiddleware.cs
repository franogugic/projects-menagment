using System.Text.Json;
using projects_menagment.Api.Dtos.Common;
using projects_menagment.Application.Exceptions;

namespace projects_menagment.Api.Middleware;

public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, ex.Code, ex.Message);
        }
        catch (ConflictException ex)
        {
            logger.LogWarning(ex, "Conflict error while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status409Conflict, ex.Code, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning(ex, "Unauthorized error while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status401Unauthorized, ex.Code, ex.Message);
        }
        catch (ForbiddenException ex)
        {
            logger.LogWarning(ex, "Forbidden error while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status403Forbidden, ex.Code, ex.Message);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Not found error while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status404NotFound, ex.Code, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string code, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ErrorResponseDto(code, message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
