using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.FollowerRelationshipUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.FollowerRelationshipUser;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FollowerRelationshipUserController(
    IUserService userService,
    IFollowerRelationshipUserService followService,
    IMapper mapper
) : Controller
{
    [HttpGet("{followedId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Exists(string followedId)
    {
        string? followerId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (followerId == null) return Unauthorized();

        bool exists = await followService.ExistsByFollowerIdAndFollowedId(followerId, followedId);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
        {
            Code = StatusCodes.Status200OK,
            Data = exists,
            Message = "",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPost("{followedId:required}/Toggle")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<FollowerRelationshipUserDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Create(string followedId)
    {
        string? followerId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (followerId == null) return Unauthorized();

        UserEntity? follower = await userService.GetUserBySid(followerId);
        if (follower == null) 
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

        UserEntity? followed = await userService.GetUserBySid(followedId);
        if (followed == null) 
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

        FollowerRelationshipUserEntity? follow = await followService.GetByFollowerIdAndFollowedId(followerId, followedId);

        if (follow != null) 
        {
            await followService.Delete(follow);
            
            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
            {
                Code = StatusCodes.Status204NoContent,
                Message = "You have been followed in user " + followed.UserName,
                Data = null,
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        FollowerRelationshipUserEntity created = await followService.Create(followerId, followedId);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<FollowerRelationshipUserDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<FollowerRelationshipUserDto>(created),
            Message = "You have been followed in user " + followed.UserName,
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<FollowerRelationshipUserDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateFollowerRelationshipUserDto dto)
    {
        string? followerId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (followerId == null) return Unauthorized();

        FollowerRelationshipUserEntity? follow = await followService.GetById(id);
        if (follow == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Resource not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (follow.FollowerId != followerId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Data = null,
                Message = "You cannot update this data",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            }); 
        }
        
        FollowerRelationshipUserEntity update = await followService.Update(follow, dto);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<FollowerRelationshipUserDto>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<FollowerRelationshipUserDto>(update),
            Message = "Data updated",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<FollowerRelationshipUserDto>))]
    public async Task<IActionResult> GetAll([FromQuery] FollowerRelationshipUserFilterParams filter)
    {
        IQueryable<FollowerRelationshipUserEntity> iQueryable = followService.Query();
        IQueryable<FollowerRelationshipUserEntity> appliedFilter = FollowerRelationshipUserFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<FollowerRelationshipUserEntity> paginatedList = await PaginatedList<FollowerRelationshipUserEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<FollowerRelationshipUserDto> dtos = mapper.Map<Page<FollowerRelationshipUserDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
    
}