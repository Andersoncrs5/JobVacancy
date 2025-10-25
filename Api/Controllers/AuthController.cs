using Api.models.dtos.Users;
using Api.models.entities;
using Api.Services.Interfaces;
using Api.Utils.Res;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController(
    ITokenService tokenService,
    IUserService userService,
    IRolesService rolesService,
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
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and Password must be provided.");
            }

            bool checkName = await userService.ExistsByUsername(dto.Name);
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

            
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}