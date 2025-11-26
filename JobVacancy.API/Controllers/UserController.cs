using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.User;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Minio.DataModel.Response;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController(
    ITokenService tokenService,
    IUserService userService,
    IRolesService rolesService,
    IConfiguration configuration,
    IMiniOService miniOService,
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

            if (!string.IsNullOrWhiteSpace(dto.Name) && await userService.ExistsByUsername(dto.Name) && user.UserName != dto.Name)
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<UserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("GetItemPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllUser([FromQuery] UserFilterParams filter)
    {
        try
        {
            string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (id == null) return Unauthorized();

            var queryable = userService.GetIQueryable();
            IQueryable<UserEntity> userEntities = UserFilterQuery.ApplyFilter(queryable, filter);
            PaginatedList<UserEntity> list = await PaginatedList<UserEntity>.CreateAsync(
                source: userEntities, 
                pageIndex: filter.PageNumber,
                pageSize: filter.PageSize    
            );

            Page<UserDto> finalResponse = mapper.Map<Page<UserDto>>(list);

            return Ok(finalResponse);
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
    [HttpPut("image/profile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> UpdateImageProfile([FromForm] UpdateUserDto dto)
    {
        string bucketName = configuration.GetValue<string>("Buckets:ImageBucketName") ?? throw new ArgumentNullException(nameof(bucketName));
        string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (id == null) return Unauthorized();
        
        UserEntity? user = await userService.GetUserBySid(id);
        if (user == null) return Unauthorized();

        if (dto.File == null || dto.File.Length == 0) 
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseHttp<object>
            {
                Data = null,
                Code = StatusCodes.Status400BadRequest,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Message = "File is required"
            });
        }

        PutObjectResponse response = await miniOService.UploadOptimized(bucketName, dto.File);
        
        if (response.Size == 0 || dto.File.Length != response.Size)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Message = "Error the upload image! Try again later.",
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                Data = null,
                Code = StatusCodes.Status500InternalServerError
            });
        }
        
        if (!string.IsNullOrWhiteSpace(user.ImageProfileUrl))
        {
            bool existsFile = await miniOService.ExistsFile(bucketName, user.ImageProfileUrl);
            if (existsFile)
                await miniOService.RemoveFile(bucketName, user.ImageProfileUrl);
        }

        user.ImageProfileUrl = response.ObjectName;
        UserResult result = await userService.UpdateAsync(user);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Data = mapper.Map<UserDto>(result.User),
            Code = StatusCodes.Status200OK,
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = "Image has been updated"
        });
    }
    */
}