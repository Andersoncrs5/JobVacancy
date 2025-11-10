using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.ReviewEnterprise;
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
public class ReviewEnterpriseController(
    IUserService userService,
    IEmployeeEnterpriseService employeeEnterpriseService,
    IReviewEnterpriseService reviewEnterpriseService,
    IEnterpriseService enterpriseService,
    IConfiguration configuration,
    IMapper mapper
) : Controller
{
    
    [HttpPost("{enterpriseId:required}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<ReviewEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Create(string enterpriseId, [FromBody] CreateReviewEnterpriseDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        if (!await enterpriseService.ExistsById(enterpriseId))
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Enterprise Not Found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        EmployeeEnterpriseEntity? employee = await employeeEnterpriseService.GetByUserIdAndEnterpriseId(userId, enterpriseId);
        if (employee == null) 
        {
            return StatusCode( StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Message = "You are not registered in this enterprise.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (await reviewEnterpriseService.ExistsByEnterpriseIdAndUserId(enterpriseId, userId))
        {
            return StatusCode( StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Message = "You already have a review to this enterprise.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        ReviewEnterpriseEntity review = await reviewEnterpriseService.Create(dto, enterpriseId, userId, employee.PositionId);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<ReviewEnterpriseDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<ReviewEnterpriseDto>(review),
            Message = "Review Created",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet("{reviewId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ReviewEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string reviewId)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        ReviewEnterpriseEntity? review = await reviewEnterpriseService.GetById(reviewId);
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

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<ReviewEnterpriseDto>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<ReviewEnterpriseDto>(review),
            Message = "Review found",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
        
    }
    
    [HttpDelete("{reviewId:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Del(string reviewId)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        ReviewEnterpriseEntity? review = await reviewEnterpriseService.GetById(reviewId);
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

        await reviewEnterpriseService.Delete(review);
        
        return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
        {
            Code = StatusCodes.Status204NoContent,
            Data = null,
            Message = "Review removed",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
        
    }
    
}