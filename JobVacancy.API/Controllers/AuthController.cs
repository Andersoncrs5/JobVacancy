using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    IConfiguration configuration,
    IMapper mapper
    ) : Controller
{

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("DeleteItemPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (id == null) return Unauthorized();

            UserEntity? user = await userService.GetUserBySid(id);
            if (user == null)
            {
                return StatusCode(404, new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            await userService.DeleteUser(user);
            
            return Ok(new ResponseHttp<object>
            {
                Data = null,
                Message = "User successfully deleted",
                Code = 200,
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<UserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("GetItemPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (id == null) return Unauthorized();

            UserEntity? user = await userService.GetUserBySid(id);
            if (user == null)
            {
                return StatusCode(404, new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            UserDto dto = mapper.Map<UserDto>(user);
            
            return Ok(new ResponseHttp<UserDto>
            {
                Data = dto,
                Code = 200,
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
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
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<ResponseTokens>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("authSystemPolicy")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
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
                    Status = false,
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
            ResponseTokens tokens = tokenService.CreateTokens(user, userRoles, configuration);
            await userService.UpdateSimple(user);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<ResponseTokens>
            {
                Data = tokens,
                Code = StatusCodes.Status201Created,
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
    
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ResponseTokens>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [EnableRateLimiting("authSystemPolicy")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            UserEntity? user = await userService.GetUserByEmail(dto.Email!);
            if (user == null) 
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status401Unauthorized,
                    Message = "Login failed.",
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }

            if (!await userService.CheckPassword(user, dto.Password!)) 
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status401Unauthorized,
                    Message = "Login failed.",
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }

            IList<string> rolesAsync = await userService.GetRolesAsync(user);

            ResponseTokens responseTokens = tokenService.CreateTokens(user, rolesAsync, configuration);
            await userService.UpdateSimple(user);
            
            return Ok(new ResponseHttp<ResponseTokens>
            {
                Data = responseTokens,
                Code = StatusCodes.Status200OK,
                Message = "Login succeeded",
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

    /*
    [HttpGet("RefreshToken/{refreshToken}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ResponseTokens>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromRoute] string refreshToken)
    {
        UserEntity? user = await userService.GetUserByRefreshToken(refreshToken);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseHttp<object>
            {
                Data = null,
                Code = StatusCodes.Status401Unauthorized,
                Message = "Refresh token failed.",
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,

            });
        }
        
        IList<string> rolesAsync = await userService.GetRolesAsync(user);

        ResponseTokens responseTokens = tokenService.CreateTokens(user, rolesAsync, configuration);
        await userService.UpdateSimple(user);
            
        return Ok(new ResponseHttp<ResponseTokens>
        {
            Data = responseTokens,
            Code = StatusCodes.Status200OK,
            Message = "Tokens created succeeded!",
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
        });
    }
    */
    
    
    
}