using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate Next , ILogger<ExceptionMiddleware> logger , IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }

        // InvokeAsync
        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                // Holds The Next MiddleWare 
                await _next.Invoke(context);

            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,ex.Message);
                // IF In Production => Log Ex In Database
                // IF In Devolpment 
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                //if(_env.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse(500,ex.Message,ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse(500);
                //}
                var Response = _env.IsDevelopment() ? (new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString() )): (new ApiExceptionResponse(500));
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var JsonResponse = JsonSerializer.Serialize(Response , Options);
                context.Response.WriteAsync(JsonResponse);
            }
        }
    }
}
