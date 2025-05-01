using BugTicketingSystem.BL.Exceptions;
using BugTicketingSystem.BL.Utils.Error;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BugTicketingSystem.APIs.Middleware
{
    public class ExceptionHandlingMiddleware : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is BLValidationException blException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(blException.Error.Select(
                    e => new APIError
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage
                    }));
            }
            else
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return true;
        }
    }

}

