using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.ApplicationVacancy;
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
public class ApplicationVacancyController(
    IUserService userService,
    IVacancyService vacancyService,
    IApplicationVacancyService applicationVacancyService,
    IMapper mapper
) : Controller
{

    [HttpGet("{vacancyId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Exists(string vacancyId)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();
        
        bool exists = await applicationVacancyService.ExistsByVacancyIdAndUserId(vacancyId, userId);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
        {
            Code = StatusCodes.Status200OK,
            Status = true,
            Data = exists,
            Message = "",
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
        });
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<ApplicationVacancyDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status410Gone, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateApplicationVacancyDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(userId);
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

        VacancyEntity? vacancy = await vacancyService.GetById(dto.VacancyId);
        if (vacancy == null) 
        {
            return StatusCode(404, new ResponseHttp<object>
            {
                Code = 404,
                Message = "Vacancy not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (vacancy.ApplicationDeadLine < DateTime.UtcNow)
        {
            return StatusCode(StatusCodes.Status410Gone, new ResponseHttp<object>
            {
                Code = StatusCodes.Status410Gone,
                Message = "The vacancy is deadline",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (vacancy.Status != VacancyStatusEnum.Open)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = $"Application denied. The vacancy status is currently {vacancy.Status}.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        bool exists = await applicationVacancyService.ExistsByVacancyIdAndUserId(dto.VacancyId, userId);
        if (exists) 
        {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Message = "You already applied to this vacancy",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        ApplicationVacancyEntity app = await applicationVacancyService.Create(dto, userId);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<ApplicationVacancyDto>
        {
            Code = StatusCodes.Status201Created,
            Status = true,
            Data = mapper.Map<ApplicationVacancyDto>(app),
            Message = "Application finished",
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
        });
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ApplicationVacancyDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        ApplicationVacancyEntity? app = await applicationVacancyService.GetById(id);
        if (app == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Application not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        return Ok(new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Status = true,
            Data = mapper.Map<ApplicationVacancyDto>(app),
            Message = "Application found",
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPatch("{id:required}")]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ApplicationVacancyDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateApplicationVacancyDto dto)
    {
        ApplicationVacancyEntity? app = await applicationVacancyService.GetById(id);
        if (app == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Application not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        ApplicationVacancyEntity update = await applicationVacancyService.Update(dto, app);
        
        return Ok(new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Status = true,
            Data = mapper.Map<ApplicationVacancyDto>(update),
            Message = "Application found",
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Delete(string id)
    {
        ApplicationVacancyEntity? app = await applicationVacancyService.GetById(id);
        if (app == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Application not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        await applicationVacancyService.Delete(app);
        
        return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
        {
            Code = StatusCodes.Status204NoContent,
            Status = true,
            Data = null,
            Message = "Application removed",
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<ApplicationVacancyDto>))]
    public async Task<IActionResult> GetAll([FromQuery] ApplicationVacancyFilterParams filter)
    {
        IQueryable<ApplicationVacancyEntity> iQueryable = applicationVacancyService.Query();
        IQueryable<ApplicationVacancyEntity> appliedFilter = ApplicationVacancyFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<ApplicationVacancyEntity> paginatedList = await PaginatedList<ApplicationVacancyEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<ApplicationVacancyDto> dtos = mapper.Map<Page<ApplicationVacancyDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}