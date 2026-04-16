using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Alumni_Portal.Exceptions
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;
        public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(

            HttpContext httpcontext,
            Exception exception,
            CancellationToken cancellationToken = default
        )
        {

            httpcontext.Response.ContentType = "application/problem+json";
            httpcontext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpcontext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().ToString(),
                    Title = "An unexpected error occured.",
                    Status=StatusCodes.Status500InternalServerError,
                    Detail = exception.Message,
                    Instance= httpcontext.Request.Path
                    
                }



            });
        }

    }
}

