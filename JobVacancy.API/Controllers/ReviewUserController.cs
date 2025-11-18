using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.ReviewUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
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

    [HttpGet("{id:guid}")]
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
    
}