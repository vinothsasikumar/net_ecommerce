namespace Ecommerce.Api.Middlwares
{
    public class ErrorLogger
    {
        private readonly RequestDelegate _next;

        public ErrorLogger(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorLogger> logger)
        {
            try
            {
                logger.LogInformation(context.Request.Path.Value.ToString());
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception has occurred while processing the request.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
            }
        }
    }
}
