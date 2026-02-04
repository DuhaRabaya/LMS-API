using LMS.DAL.DTO.Response.ErrorResponse;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace LMS.PL
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception e, CancellationToken cancellationToken)
        {
            var error = new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Server Error!!!!!!!!!",
                StackTrace= e.InnerException?.Message ?? e.Message

            };
            context.Response.StatusCode = error.StatusCode;
            await context.Response.WriteAsJsonAsync(error);

            return true;
        }
    }
}
