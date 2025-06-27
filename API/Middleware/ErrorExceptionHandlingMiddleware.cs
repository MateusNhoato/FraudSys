using FraudSys.Resources;
using FraudSys.Services;

namespace FraudSys.Middleware
{
    public class ErrorExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorExceptionHandlingMiddleware> _logger;


        public ErrorExceptionHandlingMiddleware(RequestDelegate next, ILogger<ErrorExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await HandleException(context, ex);
            }
        }


        private async Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message.ToString());

            await context.Response.WriteAsJsonAsync(FraudSysResource.OcorreuUmErroDesconhecido);
        }
    }
}
