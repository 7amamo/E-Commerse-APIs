using E_Commerce_Project.Errors;
using System.Text.Json;

namespace E_Commerce_Project.Middlewares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        public ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate Next , ILogger<ExceptionMiddleWare> logger , IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
               await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);  // Catch Massege

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                //if(_env.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse(500 , ex.Message , ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse(500);
                //}

                var Response  = _env.IsDevelopment() ?
                     new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
                     : new ApiExceptionResponse(500 , ex.Message , ex.StackTrace.ToString());
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };


                var jsonresponse = JsonSerializer.Serialize(Response , options);

                context.Response.WriteAsync(jsonresponse);

            }
        }


    }
}
