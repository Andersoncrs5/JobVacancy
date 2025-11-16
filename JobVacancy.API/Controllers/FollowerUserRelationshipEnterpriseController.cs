using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.FollowerUserRelationshipEnterprise;
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
public class FollowerUserRelationshipEnterpriseController(
    IUserService userService,
    IFollowerUserRelationshipEnterpriseService followService,
    IEnterpriseRepository enterpriseRepository,
    IMapper mapper
) : Controller
{

    [HttpGet("{enterpriseId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Exists(string enterpriseId)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        bool exists = await followService.ExistsByEnterpriseIdAndUserId(enterpriseId, userId);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
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
    
    [HttpPost("{enterpriseId}/Toggle")]
    [ProducesResponseType(StatusCodes.Status201Created,
        Type = typeof(ResponseHttp<FollowerUserRelationshipEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Toggle(string enterpriseId)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        if (!await enterpriseRepository.ExistsById(enterpriseId)) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = $"Enterprise not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        FollowerUserRelationshipEnterpriseEntity? follow = await followService.GetByEnterpriseIdAndUserId(enterpriseId, userId);
        if (follow != null) 
        {
            await followService.Delete(follow);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
            {
                Code = StatusCodes.Status200OK,
                Status = true,
                Data = null,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Message = "Now you've stopped following Enterprise."
            });
        }
 
        FollowerUserRelationshipEnterpriseEntity followCreated = await followService.Create(enterpriseId, userId);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<FollowerUserRelationshipEnterpriseDto>
        {
            Code = StatusCodes.Status201Created,
            Status = true,
            Data = mapper.Map<FollowerUserRelationshipEnterpriseDto>(followCreated),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = "Now you are following the enterprise."
        });
    }

    [HttpPatch("{id:required}")]
    [Authorize(Roles = "USER_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ResponseHttp<FollowerUserRelationshipEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateFollowerUserRelationshipEnterpriseDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        FollowerUserRelationshipEnterpriseEntity? follow = await followService.GetById(id);
        if (follow == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = $"Data not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (follow.UserId != userId) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = $"You cannot update this data",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        FollowerUserRelationshipEnterpriseEntity update = await followService.Update(dto, follow);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Status = true,
            Data = mapper.Map<FollowerUserRelationshipEnterpriseDto>(update),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = "Data updated with successfully"
        });

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<FollowerUserRelationshipEnterpriseDto>))]
    public async Task<IActionResult> GetAll([FromQuery] FollowerUserRelationshipEnterpriseFilterParam filter)
    {
        IQueryable<FollowerUserRelationshipEnterpriseEntity> iQueryable = followService.Query();
        IQueryable<FollowerUserRelationshipEnterpriseEntity> appliedFilter = FollowerUserRelationshipEnterpriseFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<FollowerUserRelationshipEnterpriseEntity> paginatedList = await PaginatedList<FollowerUserRelationshipEnterpriseEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<FollowerUserRelationshipEnterpriseDto> dtos = mapper.Map<Page<FollowerUserRelationshipEnterpriseDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}