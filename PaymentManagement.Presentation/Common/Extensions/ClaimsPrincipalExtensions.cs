using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PaymentManagement.Presentation.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value =
                user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(value))
                throw new UnauthorizedAccessException("User id not found in token");

            return Guid.Parse(value);
        }
    }
}
