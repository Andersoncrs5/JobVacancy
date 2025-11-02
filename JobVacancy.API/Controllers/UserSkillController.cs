using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.UserSkill;
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
public class UserSkillController(
    ITokenService tokenService,
    ISkillService skillService,
    IUserSkillService userSkillService,
    IRolesService rolesService,
    IConfiguration configuration,
    IMapper mapper
) : Controller
{
    [HttpGet("{skillId:required}/exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Exists(string skillId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            bool entity = await userSkillService.ExistsByUserIdAndSkillId(userId, skillId);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
            {
                Code = StatusCodes.Status200OK,
                Data = entity,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Status = true,
                Message = entity ? "Skill already is added" : "Skill was not added",
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
    
    [HttpPost("Toggle/{skillId:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<UserSkillDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Toggle(string skillId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            bool checkSkill = await skillService.ExistsById(skillId);
            if (!checkSkill)
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Skill not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            UserSkillEntity? entity = await userSkillService.GetByUserIdAndSkillId(userId, skillId);
            if (entity != null)
            {
                await userSkillService.Delete(entity);
                
                return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status204NoContent,
                    Data = null,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Status = true,
                    Message = "Skill removed with successfully!",
                    Version = 1
                });
            }

            UserSkillEntity saved = await userSkillService.CreateAsync(userId, skillId);
            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<UserSkillDto>
            {
                Code = StatusCodes.Status201Created,
                Data = mapper.Map<UserSkillDto>(saved),
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Status = true,
                Message = "Skill added with successfully!",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<UserSkillDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAllUserSkills([FromQuery] UserSkillFilterParam filter )
    {
        try
        {
            IQueryable<UserSkillEntity> iQueryable = userSkillService.Query();
            
            IQueryable<UserSkillEntity> appliedFilter = UserSkillFilterQuery.ApplyFilter(iQueryable, filter);
            PaginatedList<UserSkillEntity> paginatedList = await PaginatedList<UserSkillEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<UserSkillDto> dtos = mapper.Map<Page<UserSkillDto>>(paginatedList);
        
            return Ok(dtos);
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
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateUserSkillDto dto)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            UserSkillEntity? entity = await userSkillService.GetById(id);
            if (entity == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Data = null,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Status = true,
                    Message = "Data not found!",
                    Version = 1
                });
            }

            UserSkillEntity saved = await userSkillService.UpdateAsync(entity, dto);
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<UserSkillDto>
            {
                Code = StatusCodes.Status200OK,
                Data = mapper.Map<UserSkillDto>(saved),
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Status = true,
                Message = "Data updated with successfully!",
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
    
}