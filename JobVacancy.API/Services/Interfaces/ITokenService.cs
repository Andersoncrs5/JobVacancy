using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JobVacancy.API.models.entities;
using JobVacancy.API.Utils.Res;

namespace JobVacancy.API.Services.Interfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration config);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration config);
    ResponseTokens CreateTokens(UserEntity user, IList<string> userRoles, IConfiguration configuration);
}