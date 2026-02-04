using LMS.DAL.DTO.Response.ErrorResponse;
using LMS.PL.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace LMS.PL
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GlobalExceptionHandler(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception e, CancellationToken cancellationToken)
        {
            var error = new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = _localizer["ServerError"],
                StackTrace= e.InnerException?.Message ?? e.Message

            };
            context.Response.StatusCode = error.StatusCode;
            await context.Response.WriteAsJsonAsync(error);

            return true;
        }
    }
}
