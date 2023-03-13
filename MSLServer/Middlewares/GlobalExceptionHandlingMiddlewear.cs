using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft;

namespace MSLServer.Middlewares
{
    public class GlobalExceptionHandlingMiddleware:IMiddleware
    {
        private ILogger _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) { 
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server error has occured"
                };
                string json = JsonConvert.SerializeObject(problem);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
                
            }
        }
    }
}
