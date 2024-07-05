using FluentValidation;
using Movies.Contracts.Responses;

namespace Movies.Api.Middleware;

public class ValidationApiMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationApiMiddleware(RequestDelegate? next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new ValidationFailureResponse()
            {
                Errors = ex.Errors.Select(x => new ValidationResponse()
                {
                    PopertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };
            
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}