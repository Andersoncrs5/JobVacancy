using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobVacancy.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace JobVacancy.API.Services.Providers;

public class TokenService: ITokenService
{
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration config)
    {
        string key = config.GetSection("jwt").GetValue<string>("SecretKey") ?? 
                      throw new InvalidOperationException();

        var exp = config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes");
        
        byte[] privateKey = Encoding.UTF8.GetBytes(key);

        SigningCredentials? signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject =  new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(exp),
            Audience = config.GetSection("jwt").GetValue<string>("ValidAudience"),
            SigningCredentials = signingCredentials,
            Issuer = config.GetSection("jwt").GetValue<string>("ValidIssuer")
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }
    
    public string GenerateRefreshToken()
    {
        byte[] secureRandomBytes = new byte[(128 * 2)];

        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
       
        randomNumberGenerator.GetBytes(secureRandomBytes);

        string refreshToken = Convert.ToBase64String(secureRandomBytes);

        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration config)
    {
        string secretKey = config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid key");

        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal? main = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) 
        {
            throw new SecurityTokenException("Invalid token");
        }

        return main;
    }
    
}