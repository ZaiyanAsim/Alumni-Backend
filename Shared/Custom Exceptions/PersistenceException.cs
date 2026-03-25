using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Custom_Exceptions
{
    public sealed class PersistenceExceptionHandler : IExceptionHandler
    {
        private IProblemDetailsService _problemDetailsService;

        public PersistenceExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
    HttpContext httpcontext,
    Exception exception,
    CancellationToken cancellationToken = default)
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Database error occurred.";

            
             if (exception is DbUpdateConcurrencyException)
            {
                statusCode = StatusCodes.Status409Conflict;
                title = "Concurrency conflict occurred.";
            }
            else if (exception is DbUpdateException)
            {
                statusCode = StatusCodes.Status409Conflict;
                title = "Database constraint violation.";
            }

            httpcontext.Response.ContentType = "application/problem+json";
            httpcontext.Response.StatusCode = statusCode;

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpcontext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Title = title,
                    Status = statusCode,
                    Detail = exception.Message,
                    Instance = httpcontext.Request.Path
                }
            });
        }
    }
}