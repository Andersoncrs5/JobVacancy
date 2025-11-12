using AutoMapper;
using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.Area;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AreaController(
    IAreaService areaService,
    IMapper mapper
) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<AreaDto>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [EnableRateLimiting("CreateItemPolicy")]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateAreaDto dto)
    {
        AreaEntity newArea = await areaService.Create(dto);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<AreaDto>
        {
            Data = mapper.Map<AreaDto>(newArea),
            Message = "Area created with success.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Code = StatusCodes.Status201Created
        });
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<AreaDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        AreaEntity? area = await areaService.GetById(id);
        if (area == null) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = $"Area not found.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
            });
        }

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<AreaDto>
        {
            Data = mapper.Map<AreaDto>(area),
            Message = "Area found.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> Del(string id)
    {
        AreaEntity? area = await areaService.GetById(id);
        if (area == null) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = $"Area not found.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
            });
        }

        await areaService.Delete(area);
        
        return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
        {
            Data = null,
            Message = "Area deleted",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Code = StatusCodes.Status204NoContent
        });
    }
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<AreaDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateAreaDto dto)
    {
        AreaEntity? area = await areaService.GetById(id);
        if (area == null) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = $"Area not found.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
            });
        }

        if (
            !string.IsNullOrWhiteSpace(dto.Name) &&
            !area.Name.Equals(dto.Name, StringComparison.InvariantCultureIgnoreCase) &&
            await areaService.ExistsName(dto.Name)
        ) 
        {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<AreaDto>
            {
                Data = null,
                Message = $"The name {dto.Name} already exists.",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Code = StatusCodes.Status409Conflict
            });
        }
        
        AreaEntity update = await areaService.Update(dto, area);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<AreaDto>
        {
            Data = mapper.Map<AreaDto>(update),
            Message = "Area updated.",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<AreaDto>))]
    public async Task<IActionResult> Get([FromQuery] AreaFilterParams filter)
    {
        IQueryable<AreaEntity> iQueryable = areaService.Query();
        IQueryable<AreaEntity> appliedFilter = AreaFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<AreaEntity> paginatedList = await PaginatedList<AreaEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<AreaDto> dtos = mapper.Map<Page<AreaDto>>(paginatedList);
        
        return Ok(dtos);
    }

    [HttpGet("{name:required}/Exists/Name")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> Exists(string name)
    {
        bool exists = await areaService.ExistsName(name);
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Data = exists,
            Message = "Area deleted",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Code = StatusCodes.Status200OK
        });
    }
    
}