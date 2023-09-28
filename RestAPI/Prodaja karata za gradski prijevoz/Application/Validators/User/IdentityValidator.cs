using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Validators.User;

public static class IdentityValidator
{
    public static bool IsUserRole(string role, JwtSecurityToken jwtSecurityToken)
    {
        Claim roleClaim = jwtSecurityToken.Claims.Single(claim => claim.Type.Equals("role", StringComparison.OrdinalIgnoreCase));

        return roleClaim.Value.Equals(role, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsSameUser(Guid userId, JwtSecurityToken jwtSecurityToken)
    {
        Claim userIdClaim = jwtSecurityToken.Claims.Single(claim => claim.Type.Equals("id", StringComparison.OrdinalIgnoreCase));

        return userId == new Guid(userIdClaim.Value);
    }
}
