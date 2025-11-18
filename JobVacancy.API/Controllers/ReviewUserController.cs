using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.ReviewUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.ReviewUser;
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
public class ReviewUserController(
    IUserService userService,
    IReviewUserService reviewUserService,
    IMapper mapper
    ): Controller
{

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ReviewUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        ReviewUserEntity? review = await reviewUserService.GetById(id);
        if (review == null) 
        {
            return StatusCode( StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Review not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<ReviewUserDto>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<ReviewUserDto>(review),
            Message = "Review found",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Delete(string id)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        
        ReviewUserEntity? review = await reviewUserService.GetById(id);
        if (review == null) 
        {
            return StatusCode( StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Review not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        if (review.ActorId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "You do not have permission to delete this review",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        await reviewUserService.Delete(review);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = null,
            Message = "Review found",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateReviewUserDto dto )
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        
        ReviewUserEntity? review = await reviewUserService.GetById(id);
        if (review == null) 
        {
            return StatusCode( StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Review not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        if (review.ActorId != userId) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "You do not have permission to update this review",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        DateTime limitDate = review.CreatedAt.AddMonths(1);

        if (DateTime.UtcNow > limitDate)
        {
            return StatusCode( StatusCodes.Status410Gone, new ResponseHttp<object>
            {
                Code = StatusCodes.Status410Gone,
                Message = "Review cannot be updated",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        ReviewUserEntity update = await reviewUserService.Update(dto, review);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<ReviewUserDto>(update),
            Message = "Review updated",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<ReviewUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateReviewUserDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        if (await reviewUserService.ExistsByActorIdAndTargetId(userId, dto.TargetUserId))
        {
            return StatusCode( StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Message = "You already have a review to this user.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        ReviewUserEntity review = await reviewUserService.Create(dto, userId);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<ReviewUserDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<ReviewUserDto>(review),
            Message = "Review Created",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<ReviewUserDto>))]
    public async Task<IActionResult> GetAll([FromQuery] ReviewUserFilterParams filter)
    {
        IQueryable<ReviewUserEntity> iQueryable = reviewUserService.Query();
        IQueryable<ReviewUserEntity> appliedFilter = ReviewUserFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<ReviewUserEntity> paginatedList = await PaginatedList<ReviewUserEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<ReviewUserDto> dtos = mapper.Map<Page<ReviewUserDto>>(paginatedList);
        
        return Ok(dtos);
    }

    [HttpGet("{userId}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    public async Task<IActionResult> Exists(string userId)
    {
        string? actorId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(actorId)) return Unauthorized();

        bool exists = await reviewUserService.ExistsByActorIdAndTargetId(actorId, userId);
    
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
    
    
}