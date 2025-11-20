using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.UserEvaluation;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.UserEvaluation;
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
public class UserEvaluationController(
    IUserEvaluationService userEvaluationService,
    IEnterpriseService enterpriseService,
    IEmployeeEnterpriseService employeeEnterpriseService,
    IPositionService positionService,
    IMapper mapper
    ): Controller
{
    [HttpPost]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<UserEvaluationDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create([FromBody] CreateUserEvaluationDto dto) 
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Forbid();

        EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userId);

        if (enterprise == null) return Forbid();

        bool existsEmployee = await employeeEnterpriseService.ExistsByUserIdAndEnterpriseId(dto.TargetUserId, enterprise.Id);
        
        if (!existsEmployee) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Data = null,
                Message = "The user are not registered with employee.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        bool exists = await userEvaluationService.ExistsByEnterpriseIdAndTargetUserId(enterprise.Id, dto.TargetUserId);

        if (exists) 
        {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Data = null,
                Message = "You already created a review to this user",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        UserEvaluationEntity created = await userEvaluationService.Create(dto, enterprise.Id, userId);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<UserEvaluationDto>
        {
            Data = mapper.Map<UserEvaluationDto>(created),
            Code = StatusCodes.Status201Created,
            Message = "Review created with success.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet("{enterpriseId:required}/{userId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    public async Task<IActionResult> Exists(string enterpriseId, string userId)
    {
        bool existsEmployee = await userEvaluationService.ExistsByEnterpriseIdAndTargetUserId(enterpriseId, userId);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
        {
            Data = existsEmployee,
            Code = StatusCodes.Status200OK,
            Message = "",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<UserEvaluationDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        UserEvaluationEntity? evaluation = await userEvaluationService.GetById(id);
        
        if (evaluation == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Review not found",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<UserEvaluationDto>
        {
            Data = mapper.Map<UserEvaluationDto>(evaluation),
            Code = StatusCodes.Status200OK,
            Message = "Review found with success.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<UserEvaluationDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Del(string id) 
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userId);

        if (enterprise == null) return Forbid();
        
        UserEvaluationEntity? evaluation = await userEvaluationService.GetById(id);
        
        if (evaluation == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Review not found",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (evaluation.EnterpriseId != enterprise.Id) return Forbid();
        
        await userEvaluationService.Delete(evaluation);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<UserEvaluationDto>
        {
            Data = null,
            Code = StatusCodes.Status200OK,
            Message = "Review deleted with success.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPatch("{id:required}")]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<UserEvaluationDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateUserEvaluationDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userId);

        if (enterprise == null) return Forbid();
        
        UserEvaluationEntity? evaluation = await userEvaluationService.GetById(id);
        
        if (evaluation == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Review not found",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (evaluation.EnterpriseId != enterprise.Id) return Forbid();

        if (
            !string.IsNullOrWhiteSpace(dto.PositionId) && 
            evaluation.PositionId != dto.PositionId &&
            !await positionService.ExistsById(dto.PositionId)
            )
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Position not found",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        UserEvaluationEntity update = await userEvaluationService.Update(dto, evaluation);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<UserEvaluationDto>
        {
            Data = mapper.Map<UserEvaluationDto>(update),
            Code = StatusCodes.Status200OK,
            Message = "Review updated with success.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<UserEvaluationDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] UserEvaluationFilterParams filter)
    {
        IQueryable<UserEvaluationEntity> iQueryable = userEvaluationService.Query();
        IQueryable<UserEvaluationEntity> appliedFilter = UserEvaluationFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<UserEvaluationEntity> paginatedList = await PaginatedList<UserEvaluationEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
    
        Page<UserEvaluationDto> dtos = mapper.Map<Page<UserEvaluationDto>>(paginatedList);
    
        return Ok(dtos);
    }
    
}