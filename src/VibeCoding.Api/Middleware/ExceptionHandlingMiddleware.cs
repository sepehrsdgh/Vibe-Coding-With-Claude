using System.Net;
using VibeCoding.Application.Common.Exceptions;
using VibeCoding.Domain.Common;

namespace VibeCoding.Api.Middleware;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static void UseExceptionHandlingMiddleware(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                var (statusCode, title) = ex switch
                {
                    NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                    DomainException => (HttpStatusCode.BadRequest, ex.Message),
                    _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
                };

                await Results.Problem(title: title, statusCode: (int)statusCode).ExecuteAsync(context);
            }
        });
    }
}
