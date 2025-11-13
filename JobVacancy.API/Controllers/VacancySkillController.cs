using AutoMapper;
using JobVacancy.API.models.dtos.VacancySkill;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.VacancySkill;
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
public class VacancySkillController(
    IUserService userService,
    IEnterpriseService enterpriseService,
    IVacancyService vacancyService,
    IVacancySkillService vacancySkillService,
    ISkillService skillService,
    IAreaService areaService,
    IMapper mapper
) : Controller
{
    [HttpGet("{vacancyId:required}/{skillId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> ExistsAsync(string vacancyId, string skillId) 
    {
        bool exists = await vacancySkillService.ExistsByVacancyIdAndSkillId(vacancyId, skillId);
        
        return Ok(new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = exists,
            Message = "",
            Status = true,
            Version = 1,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier
        });
    }

    [HttpPost]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<VacancySkillDto>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create([FromBody] CreateVacancySkillDto dto)
    {
        bool exists = await vacancySkillService.ExistsByVacancyIdAndSkillId(dto.VacancyId, dto.SkillId);
        if (exists) 
        {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Data = null,
                Message = "This skill already is added.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        VacancySkillEntity entity = await vacancySkillService.CreateAsync(dto);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<object>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<VacancySkillDto>(entity),
            Message = $"Skill added with successfully.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<VacancySkillDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        VacancySkillEntity? skill = await vacancySkillService.GetById(id);
        if (skill == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Skill not found.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<VacancySkillDto>(skill),
            Message = $"Skill found with successfully.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPatch("{id:required}")]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<VacancySkillDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateVacancySkillDto dto )
    {
        VacancySkillEntity? skill = await vacancySkillService.GetById(id);
        if (skill == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Skill not found.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (
            !string.IsNullOrWhiteSpace(dto.SkillId) && 
            skill.SkillId != dto.SkillId && 
            !await skillService.ExistsById(dto.SkillId)
            ) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Data = null,
                Code = StatusCodes.Status404NotFound,
                Message = "Skill not found.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        VacancySkillEntity update = await vacancySkillService.UpdateAsync(dto, skill);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<VacancySkillDto>(update),
            Message = $"Skill updated with successfully.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Del(string id)
    {
        VacancySkillEntity? skill = await vacancySkillService.GetById(id);
        if (skill == null) 
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Skill not found.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        await vacancySkillService.Delete(skill);
        
        return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
        {
            Code = StatusCodes.Status204NoContent,
            Data = null,
            Message = $"Skill removed with successfully.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<VacancySkillDto>))]
    public async Task<IActionResult> GetAll([FromQuery] VacancySkillFilterParams  filter)
    {
        IQueryable<VacancySkillEntity> iQueryable = vacancySkillService.Query();
        IQueryable<VacancySkillEntity> appliedFilter = VacancySkillFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<VacancySkillEntity> paginatedList = await PaginatedList<VacancySkillEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<VacancySkillDto> dtos = mapper.Map<Page<VacancySkillDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}