using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController(
    ITokenService tokenService,
    IUserService userService,
    IRolesService rolesService,
    IConfiguration configuration
    ) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("authSystemPolicy")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto )
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = dto.Email.Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dto.PasswordHash))
            {
                return BadRequest("Email and Password must be provided.");
            }

            bool checkName = await userService.ExistsByUsername(dto.Username);
            if (checkName == true)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status409Conflict,
                    Message = "Username already exists.",
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }
            
            bool checkEmail = await userService.ExistsByEmail(dto.Email);
            if (checkEmail == true)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status409Conflict,
                    Message = "Email already exists.",
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }
            
            RoleEntity? role = await rolesService.GetByName("USER_ROLE");
            if (role == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = "No role found with name USER_ROLE.",
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }

            UserResult result = await userService.CreateAsync(dto);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHttp<IEnumerable<string>>
                {
                    Message = "Error creating new user",
                    Data = result.Errors,
                    Code = 400,
                    Timestamp = DateTimeOffset.Now,
                    Status = false,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                });
            }
            
            UserEntity? user = await userService.GetUserByEmail(dto.Email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status404NotFound,
                    Message = "User not found.",
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }

            UserResult addRoleResult = await userService.AddRoleToUser(user, role);
            if (!addRoleResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<IEnumerable<string>>
                {
                    Message = "Error adding roles",
                    Data = addRoleResult.Errors,
                    Code = 500,
                    Timestamp = DateTimeOffset.Now,
                    Status = false,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                });
            }

            IList<string> userRoles = await userService.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            foreach (string userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            JwtSecurityToken token = tokenService.GenerateAccessToken(claims, configuration);
            string refreshToken = tokenService.GenerateRefreshToken();
            _ = int.TryParse(configuration["jwt:RefreshTokenValidityInMinutes"], out int refreshTokenLifetime);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenLifetime);
            await userService.UpdateSimple(user);

            ResponseTokens tokens = new ResponseTokens
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                ExpiredAt = token.ValidTo,
                ExpiredAtRefreshToken = user.RefreshTokenExpiryTime
            };
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<ResponseTokens>
            {
                Data = tokens,
                Code = StatusCodes.Status200OK,
                Message = "Welcome",
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
            });

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Hello World!");
    }
    
}