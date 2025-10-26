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
public class UserController(
    ITokenService tokenService,
    IUserService userService,
    IRolesService rolesService,
    IConfiguration configuration,
    IMapper mapper
    ) : Controller
{
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<UserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [EnableRateLimiting("UpdateItemPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
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
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }
 
            UserResult userUpdated = await userService.UpdateAsync(user, dto);

            UserDto userUpdateDto = mapper.Map<UserDto>(userUpdated.User);
            
            return Ok(new ResponseHttp<UserDto>
            {
                Data = userUpdateDto,
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
    
}