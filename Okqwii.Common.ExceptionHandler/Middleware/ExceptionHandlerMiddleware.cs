using Microsoft.AspNetCore.Http;
using Okqwii.Common.ExceptionHandler.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Okqwii.Common.ExceptionHandler.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        //Delegate is called in every request
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">A Request Delegate</param>
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokation 
        /// </summary>
        /// <param name="httpContext">The HTTP Context</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext) {
        
            try
            {
                //Proceeds in the pipeline
                await _next(httpContext);
            }           
            catch(Exception exception)
            {
                //In case there's an exception
                await HandleException(httpContext, exception);
            }
        }

        /// <summary>
        /// Handles the Exceptions
        /// </summary>
        /// <param name="httpContext">The Http context</param>
        /// <param name="exception">The Exception that was detected</param>
        /// <returns>A Json object with status code and message of the exception</returns>
        public async Task HandleException(HttpContext httpContext, Exception exception)
        {
            //Gets the response to be sent
            var response = httpContext.Response;
            response.ContentType = "application/json";

            //Defines the status code of the response
            response.StatusCode = exception switch
            {
                BadRequestException => (int)HttpStatusCode.BadRequest,
                ForbiddenException => (int)HttpStatusCode.Forbidden,
                InternalException => (int)HttpStatusCode.InternalServerError,
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                //Default
                _ => (int)HttpStatusCode.InternalServerError
            };

            //Serialize error message and code
            await response.WriteAsync(JsonSerializer.Serialize(new { ErrorCode = response.StatusCode, Message = exception.Message }));
        }
    }
}
