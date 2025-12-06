namespace PaymentManagement.Presentation.Middlewares
{
    public class JwtRateLimit
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtRateLimit(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration.GetRequiredSection("JwtSettings");
        }

        public async Task InvokeAsync(HttpContext context)
        {

        }

    }
}
