using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Custom_Exceptions.ExceptionClasses;

namespace Shared.Custom_Exceptions
{
    public sealed class ValidationExceptionHandler : IExceptionHandler
    {
        private IProblemDetailsService _problemDetailsService;

        public ValidationExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpcontext,
            Exception exception,
            CancellationToken cancellationToken = default
        )
        {
            if (exception is not ValidationException validationException)
            {
                return false;
            }
            httpcontext.Response.ContentType = "application/problem+json";
            httpcontext.Response.StatusCode = StatusCodes.Status400BadRequest;
            

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpcontext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    
                    Title = "Validation Error.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message,
                    Extensions =
                    {
                        ["errors"] = validationException.Errors
                    },
                    Instance = httpcontext.Request.Path,
                    
                },
                    
               
                
            });
            
        }       
    }
}
