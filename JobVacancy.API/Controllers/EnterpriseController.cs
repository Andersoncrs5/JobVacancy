using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.Enterprise;
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
public class EnterpriseController(
    IEnterpriseService enterpriseService,
    IRolesService rolesService,
    IUserService userService,
    IConfiguration configuration,
    IMapper mapper
) : Controller
{
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Del(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Forbid();
            
            EnterpriseEntity? entity = await enterpriseService.GetById(id);
            if (entity == null)
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
            
            if (entity.UserId != userId) 
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status403Forbidden,
                    Message = "You do not have permission to delete this enterprise",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            enterpriseService.Delete(entity);
            
            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<EnterpriseDto>
            {
                Code = StatusCodes.Status204NoContent,
                Data = null,
                Message = "Enterprise deleted with success.",
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
    
    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            EnterpriseEntity? entity = await enterpriseService.GetById(id);
            if (entity == null)
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
            
            EnterpriseDto enterpriseDto = mapper.Map<EnterpriseDto>(entity);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<EnterpriseDto>
            {
                Code = 200,
                Data = enterpriseDto,
                Message = "Enterprise found with success.",
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
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<EnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateEnterpriseDto dto)
    {
        try
        {
            var datasRoles = configuration.GetSection("Roles");
            string userRole = datasRoles["UserRole"] ?? throw new InvalidOperationException("User role configuration is missing.");
            string enterpriseRole = datasRoles["EnterpriseRole"] ?? throw new InvalidOperationException("User role configuration is missing.");
            
            var roleEnterprise = await rolesService.GetByName(enterpriseRole) ?? throw new InvalidOperationException("Role enterprise configuration is missing.");
            
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Forbid();

            UserEntity? user = await userService.GetUserBySid(userId);
            if (user == null) return Forbid();
            
            bool checkName = await enterpriseService.ExistsByName(dto.Name);
            if (checkName)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = "Name is taken",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            bool checkUserId = await enterpriseService.ExistsByUserId(userId);
            if (checkUserId)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = "You already have an enterprise",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            EnterpriseEntity enterprise = await enterpriseService.CreateAsync(dto, userId);
            
            UserResult removeRoleToUser = await userService.RemoveRoleToUser(user, userRole);
            UserResult addRoleToUser = await userService.AddRoleToUser(user, roleEnterprise);
            
            if (!removeRoleToUser.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
                {
                    Message = $"Error the remove role {userRole} of user {user.UserName}",
                    Code = StatusCodes.Status500InternalServerError,
                    Data = removeRoleToUser.Errors,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (!addRoleToUser.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
                {
                    Message = $"Error the to add role {enterpriseRole} of user {user.UserName}",
                    Code = StatusCodes.Status500InternalServerError,
                    Data = addRoleToUser.Errors,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<EnterpriseDto>
            {
                Data = mapper.Map<EnterpriseDto>(enterprise),
                Code = 201,
                Message = "Enterprise created with success.",
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
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Page<EnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] EnterpriseFilterParam filter)
    {
        try
        {
            IQueryable<EnterpriseEntity> queryable = enterpriseService.Query();
            IQueryable<EnterpriseEntity> enterpriseFiltered = EnterpriseFilterQuery.ApplyFilter(queryable, filter);
            PaginatedList<EnterpriseEntity> list = await PaginatedList<EnterpriseEntity>.CreateAsync(
                source:  enterpriseFiltered,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
            
            Page<EnterpriseDto> dtos = mapper.Map<Page<EnterpriseDto>>(list);
        
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateEnterpriseDto dto)
    {
        try
        {
            EnterpriseEntity? entity = await enterpriseService.GetById(id);
            if (entity == null)
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

            if (
                !string.IsNullOrEmpty(dto.Name) && 
                dto.Name != entity.Name &&
                await enterpriseService.ExistsByName(dto.Name)
                )
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = "Name is taken",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            var updateAsync = await enterpriseService.UpdateAsync(entity, dto);

            EnterpriseDto enterpriseDto = mapper.Map<EnterpriseDto>(updateAsync);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<EnterpriseDto>
            {
                Code = 200,
                Data = enterpriseDto,
                Message = "Enterprise found with success.",
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
    
}