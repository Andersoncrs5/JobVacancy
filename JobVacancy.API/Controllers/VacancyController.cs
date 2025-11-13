using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.Vacancy;
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
public class VacancyController(
    IUserService userService,
    IEnterpriseService enterpriseService,
    IVacancyService vacancyService,
    IAreaService areaService,
    IMapper mapper
) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<VacancyDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateVacancyDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(userId);
        if (user == null)
        {
            return StatusCode(404, new ResponseHttp<object?>
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

        EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userId);
        if (enterprise == null)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "Access denied. User is not linked to a valid enterprise.", // Mensagem clara
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        VacancyEntity vacancyEntity = await vacancyService.Create(dto, enterprise.Id);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<VacancyDto>
        {
            Data = mapper.Map<VacancyDto>(vacancyEntity),
            Code = StatusCodes.Status201Created,
            Message = "Vacancy created",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<VacancyDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        VacancyEntity? vacancy = await vacancyService.GetById(id);
        if (vacancy == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object?>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Vacancy not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<VacancyDto>
        {
            Data = mapper.Map<VacancyDto>(vacancy),
            Code = StatusCodes.Status200OK,
            Message = "Vacancy found",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
        });
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Delete(string id)
    {
        VacancyEntity? vacancy = await vacancyService.GetById(id);
        if (vacancy == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object?>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Vacancy not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        await vacancyService.Delete(vacancy);

        return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
        {
            Data = null,
            Code = StatusCodes.Status204NoContent,
            Message = "Vacancy deleted",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
        });
    }
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<VacancyDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateVacancyDto dto)
    {
        VacancyEntity? vacancy = await vacancyService.GetById(id);
        if (vacancy == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object?>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Vacancy not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (
            !string.IsNullOrWhiteSpace(dto.AreaId) &&
            !string.Equals(vacancy.AreaId, dto.AreaId, StringComparison.InvariantCultureIgnoreCase) &&
            !await areaService.ExistsById(dto.AreaId)
            ) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Area not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        VacancyEntity update = await vacancyService.Update(dto, vacancy);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<VacancyDto>
        {
            Data = mapper.Map<VacancyDto>(update),
            Code = StatusCodes.Status200OK,
            Message = "Vacancy found",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<VacancyDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetCategories([FromQuery] VacancyFilterParam filter)
    {
        IQueryable<VacancyEntity> iQueryable = vacancyService.Query();
        IQueryable<VacancyEntity> appliedFilter = VacancyFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<VacancyEntity> paginatedList = await PaginatedList<VacancyEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<VacancyDto> dtos = mapper.Map<Page<VacancyDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}