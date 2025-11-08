using AutoMapper;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.Position;
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
public class PositionController(
    IConfiguration configuration,
    IPositionService positionService,
    IMapper mapper
) : Controller
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<PositionDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE")]
    public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDto dto)
    {
        try
        {
            var position = await positionService.Create(dto);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<PositionDto>
            {
                Data = mapper.Map<PositionDto>(position),
                Message = "Position created with success.",
                Status = true,
                Code = StatusCodes.Status201Created,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<PositionDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            PositionEntity? position = await positionService.GetById(id);

            if (position == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Data = mapper.Map<PositionDto>(position),
                    Message = "Position not found.",
                    Status = false,
                    Code = StatusCodes.Status404NotFound,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<PositionDto>
            {
                Data = mapper.Map<PositionDto>(position),
                Message = "Position found with success.",
                Status = true,
                Code = StatusCodes.Status200OK,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            PositionEntity? position = await positionService.GetById(id);

            if (position == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Data = mapper.Map<PositionDto>(position),
                    Message = "Position not found.",
                    Status = false,
                    Code = StatusCodes.Status404NotFound,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            await positionService.Delete(position);

            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
            {
                Data = null,
                Message = "Position deleted with success.",
                Status = true,
                Code = StatusCodes.Status204NoContent,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<PositionDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdatePositionDto dto)
    {
        try
        {
            PositionEntity? position = await positionService.GetById(id);

            if (position == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Data = mapper.Map<PositionDto>(position),
                    Message = "Position not found.",
                    Status = false,
                    Code = StatusCodes.Status404NotFound,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (
                !string.IsNullOrEmpty(dto.Name) && 
                await positionService.ExistsByName(dto.Name) &&
                dto.Name != position.Name
                )
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Data = null,
                    Message = $"Name {dto.Name} already exists.",
                    Status = false,
                    Code = StatusCodes.Status409Conflict,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            PositionEntity update = await positionService.Update(dto, position);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<PositionDto>
            {
                Data = mapper.Map<PositionDto>(position),
                Message = "Position updated with success.",
                Status = true,
                Code = StatusCodes.Status200OK,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }
    
    [HttpGet("{name:required}/exists/name")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "MASTER_ROLE")]
    public async Task<IActionResult> ExistsByName(string name)
    {
        try
        {
            bool check = await positionService.ExistsByName(name);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
            {
                Code = 200,
                Data = check,
                Message = check ? $"Position exists with name {name}" : $"Position not exists with name {name}",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Data = e.StackTrace,
                Code = StatusCodes.Status500InternalServerError,
                Message = e.Message,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
            });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<PositionDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] PositionFilterParams filter)
    {
        IQueryable<PositionEntity> iQueryable = positionService.Query();
        IQueryable<PositionEntity> appliedFilter = PositionFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<PositionEntity> paginatedList = await PaginatedList<PositionEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<PositionDto> dtos = mapper.Map<Page<PositionDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}