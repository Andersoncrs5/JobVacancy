using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.EnterpriseFollowsUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.EnterpriseFollowsUser;
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
public class EnterpriseFollowsUserController(
    IUserService userService,
    IEnterpriseFollowsUserService followService,
    IEnterpriseService enterpriseService,
    IMapper mapper
) : Controller
{

    [HttpPost("{userId:required}")]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<EnterpriseFollowsUserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create(string userId)
    {
        string? userEnterprise = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userEnterprise)) return Forbid();

        EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userEnterprise);
        if (enterprise  == null) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "You have not enterprise",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        UserEntity? user = await userService.GetUserBySid(userId);
        if (user == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "User not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        EnterpriseFollowsUserEntity? follow = await followService.GetByEnterpriseIdAndUserId(enterprise.Id, userId);

        if (follow != null) 
        {
            await followService.Delete(follow);

            return Ok(new ResponseHttp<object>
            {
                Code = StatusCodes.Status200OK,
                Status = true,
                Data = null,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Message = "The following user has been deleted"
            });
        }

        EnterpriseFollowsUserEntity created = await followService.Create(enterprise.Id, userId);
        
        return StatusCode(StatusCodes.Status201Created ,new ResponseHttp<object>
        {
            Code = StatusCodes.Status201Created,
            Status = true,
            Data = mapper.Map<EnterpriseFollowsUserDto>(created),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = "The following user has been created"
        });
    }

    [HttpGet("{userId:required}/{enterpriseId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    public async Task<IActionResult> Exists(string userId, string enterpriseId)
    {
        bool exists = await followService.ExistsByEnterpriseIdAndUserId(enterpriseId, userId);

        return StatusCode(StatusCodes.Status200OK ,new ResponseHttp<bool>
        {
            Code = StatusCodes.Status200OK,
            Status = true,
            Data = exists,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = ""
        });
    }

    [HttpPatch("{followId:required}")]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EnterpriseFollowsUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Patch(string followId, UpdateEnterpriseFollowsUserDto dto)
    {
        EnterpriseFollowsUserEntity? follow = await followService.GetById(followId);
        if (follow == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Status = false,
                Message = "You are not following this user",
                Data = null,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        string? userEnterpriseId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userEnterpriseId)) return Forbid();

        EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userEnterpriseId);
        if (enterprise  == null) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "You have not an enterprise",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (follow.EnterpriseId != enterprise.Id) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "You have not enterprise",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        EnterpriseFollowsUserEntity update = await followService.Update(dto, follow);
        
        return StatusCode(StatusCodes.Status200OK ,new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Status = true,
            Data = mapper.Map<EnterpriseFollowsUserDto>(update),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = "Data updated with successfully"
        });
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<EnterpriseFollowsUserDto>))]
    public async Task<IActionResult> GetAll([FromQuery] EnterpriseFollowsUserFilterParam filter)
    {
        IQueryable<EnterpriseFollowsUserEntity> iQueryable = followService.Query();
        IQueryable<EnterpriseFollowsUserEntity> appliedFilter = EnterpriseFollowsUserFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<EnterpriseFollowsUserEntity> paginatedList = await PaginatedList<EnterpriseFollowsUserEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<EnterpriseFollowsUserDto> dtos = mapper.Map<Page<EnterpriseFollowsUserDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}