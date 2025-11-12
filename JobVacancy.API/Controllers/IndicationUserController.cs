using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.IndicationUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.IndicationUser;
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
public class IndicationUserController(
    IConfiguration configuration,
    IUserService userService,
    IIndicationUserService indicationUserService,
    IMapper mapper
) : Controller
{
    [HttpGet("{endorsedId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<bool>))]
    public async Task<IActionResult> ExistsIndicationUser(string endorsedId)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();
        
        bool exists = await indicationUserService.ExistsByEndorserIdAndEndorsedId(userId, endorsedId);
        
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
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<IndicationUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create([FromBody] CreateIndicationUserDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        bool exists = await indicationUserService.ExistsByEndorserIdAndEndorsedId(userId, dto.EndorsedId);
        if (exists) {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Status = false,
                Code = StatusCodes.Status409Conflict,
                Data = null,
                Message = "You already indicated this user",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        IndicationUserEntity entity = await indicationUserService.Create(dto, userId, dto.EndorsedId);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<IndicationUserDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<IndicationUserDto>(entity),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Message = "Indication User created",
            Status = true
        });
    }

    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(string id)
    {
        IndicationUserEntity? indication = await indicationUserService.GetById(id);
        if (indication == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Indication User not found",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false
            });
        }

        await indicationUserService.Delete(indication);
        
        return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
        {
            Code = StatusCodes.Status204NoContent,
            Data = null,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Status = true,
            Message = "Indication removed"
        });
        
    }
    
    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<IndicationUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(string id)
    {
        IndicationUserEntity? indication = await indicationUserService.GetById(id);
        if (indication == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Indication User not found",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false
            });
        }
        
        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<IndicationUserDto>(indication),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Status = true,
            Message = "Indication found"
        });
        
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<IndicationUserDto>))]
    public async Task<IActionResult> GetAll([FromQuery] IndicationUserFilterParam filter)
    {
        IQueryable<IndicationUserEntity> iQueryable = indicationUserService.Query();
        IQueryable<IndicationUserEntity> appliedFilter = IndicationUserFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<IndicationUserEntity> paginatedList = await PaginatedList<IndicationUserEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<IndicationUserDto> dtos = mapper.Map<Page<IndicationUserDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<IndicationUserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateIndicationUserEndorserDto endorserDto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();
        
        IndicationUserEntity? indication = await indicationUserService.GetById(id);
        if (indication == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Indication User not found",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false
            });
        }
        
        if (indication.EndorserId != userId)
        {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Data = null,
                Message = "You do not have permission to modify this endorsement.",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false
            });
        }
        
        IndicationUserEntity update = await indicationUserService.Update(endorserDto, indication);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<IndicationUserDto>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<IndicationUserDto>(update),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Status = true,
            Message = "Indication updated"
        });
        
    }

    
    [HttpPatch("{id:required}/By/Endorsed")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<IndicationUserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> PatchByEndorsed(string id, [FromBody] UpdateIndicationUserEndorsedDto endorserDto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();
        
        IndicationUserEntity? indication = await indicationUserService.GetById(id);
        if (indication == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Indication User not found",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false
            });
        }
        
        if (indication.EndorsedId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Data = null,
                Message = "You do not have permission to modify this endorsement.",
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Status = false
            });
        }
        
        IndicationUserEntity update = await indicationUserService.UpdateByEndorsed(endorserDto, indication);

        return StatusCode(StatusCodes.Status200OK, new ResponseHttp<IndicationUserDto>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<IndicationUserDto>(update),
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
            Status = true,
            Message = "Indication updated"
        });
        
    }

    
}