using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PaymentManagement.Presentation.Middlewares
{
    public class AccessTokenRefresh
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public AccessTokenRefresh(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration.GetRequiredSection("JwtSettings");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (token == null)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["SecretKey"]);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["Audience"],
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    // Optionally attach user info to context
                    var jwtToken = (JwtSecurityToken)validatedToken;

                    DateTime? issueDate = jwtToken.IssuedAt;
                    DateTime currentTime = DateTime.UtcNow;

                    if(issueDate.HasValue && currentTime - issueDate > TimeSpan.FromMinutes(30))
                    {

                    }

                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token");
                    return;
                }
            }
            else
            {
                // no token
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token missing");
                return;
            }

            await _next(context);
        }
    }
    
}
