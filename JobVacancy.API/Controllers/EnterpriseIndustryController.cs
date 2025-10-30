using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.EnterpriseIndustry;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EnterpriseIndustryController(
    ITokenService tokenService,
    IEnterpriseService enterpriseService,
    IEnterpriseIndustryService enterpriseIndustryService,
    IIndustryService industryService,
    IRolesService rolesService,
    IUserService userService,
    IConfiguration configuration,
    IMapper mapper
    ) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateEnterpriseIndustryDto dto) 
    {
        try
        {   
            string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (id == null) return Unauthorized();
            
            EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(id);
            if (enterprise == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Enterprise not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            IndustryEntity? industry = await industryService.GetByIdAsync(dto.IndustryId);
            if (industry == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Industry not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (industry.IsActive == false) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Industry is disabled",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            int checkAmount =  await enterpriseIndustryService.CheckAmount(dto.EnterpriseId);
            if (checkAmount > 15)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Exceed the amount max",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            dto.EnterpriseId = enterprise.Id;
            EnterpriseIndustryEntity entity = await enterpriseIndustryService.CreateAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<string>
            {
                Code = StatusCodes.Status201Created,
                Data = entity.Id,
                Message = "Industry added with successfully",
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

    [HttpDelete("{industryId:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Delete(string industryId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            var existsEnterprise = await enterpriseService.GetByUserId(userId);
            if (existsEnterprise == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Enterprise not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            EnterpriseIndustryEntity? industry = await enterpriseIndustryService.GetByIndustryIdAndEnterpriseId(industryId, existsEnterprise.Id);
            if (industry == null)
            {
                return StatusCode(statusCode: StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Data not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            enterpriseIndustryService.Delete(industry);

            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
            {
                Code = StatusCodes.Status204NoContent,
                Data = null,
                Message = "Industry removed",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<EnterpriseIndustryDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] EnterpriseIndustryFilterParams filter)
    {
        try
        {
            IQueryable<EnterpriseIndustryEntity> query = enterpriseIndustryService.GetQuery();
            IQueryable<EnterpriseIndustryEntity> filters = EnterpriseIndustryFilterQuery.ApplyFilters(query, filter);
            PaginatedList<EnterpriseIndustryEntity> list = await PaginatedList<EnterpriseIndustryEntity>.CreateAsync(
                source: filters,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
            
            Page<EnterpriseIndustryDto> dtos = mapper.Map<Page<EnterpriseIndustryDto>>(list);
        
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

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EnterpriseIndustryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            EnterpriseIndustryEntity? entity = await enterpriseIndustryService.GetByIdAsync(id);
            if (entity == null)
            {
                return StatusCode(statusCode: StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Data not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            EnterpriseIndustryDto dto = mapper.Map<EnterpriseIndustryDto>(entity);
            
            return Ok(new ResponseHttp<EnterpriseIndustryDto>
            {
                Code = StatusCodes.Status200OK,
                Data = dto,
                Message = "Data found",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
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

    [HttpPatch("{id:required}/toggle/status/is-primary")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> TogglePrimary(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            var entity = await enterpriseIndustryService.GetByIdAsync(id);
            if (entity == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Data not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            entity.IsPrimary = !entity.IsPrimary;
            EnterpriseIndustryEntity simple = await enterpriseIndustryService.UpdateSimple(entity);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
            {
                Code = StatusCodes.Status200OK,
                Data = simple.IsPrimary,
                Message = "Status is primary changed with successfully",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
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

    [HttpPost("{industryId:required}/toggle")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Toggle(string industryId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userId);
            if (enterprise == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Status = false,
                    Message = "Enterprise not found",
                    Data = null,
                    Code = StatusCodes.Status404NotFound,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            EnterpriseIndustryEntity? entity = await enterpriseIndustryService.GetByIndustryIdAndEnterpriseId(industryId, enterprise.Id);
            if (entity == null)
            {
                CreateEnterpriseIndustryDto dto = new CreateEnterpriseIndustryDto
                {
                    IndustryId = industryId,
                    EnterpriseId = enterprise.Id,
                };

                EnterpriseIndustryEntity saved = await enterpriseIndustryService.CreateAsync(dto);

                return StatusCode(StatusCodes.Status201Created, new ResponseHttp<string>
                {
                    Data = saved.Id,
                    Status = true,
                    Message = "Industry added with successfully",
                    Code = 201,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1,
                    Timestamp = DateTimeOffset.UtcNow,
                });
            }
            
            enterpriseIndustryService.Delete(entity);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
            {
                Data = null,
                Status = true,
                Message = "Industry removed with successfully",
                Code = StatusCodes.Status200OK,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1,
                Timestamp = DateTimeOffset.UtcNow,
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