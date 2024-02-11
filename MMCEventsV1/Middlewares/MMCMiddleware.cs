namespace MMCEventsV1.Middlewares
{
    public class MMCMiddleware
    {
        private readonly RequestDelegate _next;
        public MMCMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Perform any actions you want before the request reaches your endpoints

            // Check if the request's origin header is present
            var origin = context.Request.Headers.Host.ToString();
            if (IsAllowedOrigin(origin))
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            else
            {
                // If the request origin is not allowed, return a bad request
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Bad request - Origin not allowed.");
            }

            // Perform any actions you want after the request has been processed by your endpoints
        }

        private bool IsAllowedOrigin(string origin)
        {
            // Add your logic to check if the origin is allowed
            // For example, you can check against a list of allowed origins
            var allowedOrigins = new List<string> { "localhost:7187" };

            if (allowedOrigins.Contains(origin))
            {
                return true;
            }
            return  false;
        }
    }
}
