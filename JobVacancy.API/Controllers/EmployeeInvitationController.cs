using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.EmployeeInvitation;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmployeeInvitationController(
    IEnterpriseService enterpriseService,
    IUserService userService,
    IEmployeeInvitationService employeeInvitationService,
    IConfiguration configuration,
    IMapper mapper
) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<EmployeeInvitationDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeInvitationDto dto)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            EnterpriseEntity? enterprise = await enterpriseService.GetByUserId(userId);
            if (enterprise == null) 
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Enterprise Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            UserEntity? userGuest = await userService.GetUserBySid(dto.UserId);
            if (userGuest == null) 
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "User Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            EmployeeInvitationEntity invitation = await employeeInvitationService.Create(dto, enterprise.Id, userId);

            EmployeeInvitationDto map = mapper.Map<EmployeeInvitationDto>(invitation);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<EmployeeInvitationDto>
            {
                Code = StatusCodes.Status201Created,
                Data = map,
                Message = "Invitation sent",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EmployeeInvitationDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            EmployeeInvitationEntity? employee = await employeeInvitationService.GetById(id);
            if (employee == null)
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Employee Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            EmployeeInvitationDto map = mapper.Map<EmployeeInvitationDto>(employee);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<EmployeeInvitationDto>
            {
                Code = StatusCodes.Status200OK,
                Data = map,
                Message = "Invitation found",
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
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Del(string id)
    {
        try
        {
            EmployeeInvitationEntity? employee = await employeeInvitationService.GetById(id);
            if (employee == null)
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Employee Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            await employeeInvitationService.Delete(employee);

            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
            {
                Code = StatusCodes.Status204NoContent,
                Data = null,
                Message = "Invitation removed",
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
    
    [HttpPatch("{id:required}/By/Enterprise")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EmployeeInvitationDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> UpdateByEnterprise(string id, [FromBody] UpdateEmployeeInvitationDto dto)
    {
        try
        {
            EmployeeInvitationEntity? employee = await employeeInvitationService.GetById(id);
            if (employee == null)
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Employee Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (employee.Status != StatusEnum.Pending) 
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status403Forbidden,
                    Message = $"This invitation cannot be updated",
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            EmployeeInvitationEntity update = await employeeInvitationService.Update(dto, employee);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<EmployeeInvitationDto>
            {
                Code = StatusCodes.Status200OK,
                Data = mapper.Map<EmployeeInvitationDto>(update),
                Message = "Invitation updated",
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
    
    [HttpPatch("{id:required}/By/User")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EmployeeInvitationDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> UpdateByUser(string id, [FromBody] UpdateEmployeeInvitationByUserDto dto)
    {
        try
        {
            EmployeeInvitationEntity? employee = await employeeInvitationService.GetById(id);
            if (employee == null) 
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Employee Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (employee.Status == StatusEnum.Accepted) 
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status403Forbidden,
                    Message = $"This invitation cannot be updated",
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (employee.ExpiresAt < DateTime.UtcNow)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
                {
                    Status = false,
                    Code = StatusCodes.Status403Forbidden,
                    Data = null,
                    Message = $"This invitation is expired",
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            EmployeeInvitationEntity update = await employeeInvitationService.UpdateByUser(dto, employee);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<EmployeeInvitationDto>
            {
                Code = StatusCodes.Status200OK,
                Data = mapper.Map<EmployeeInvitationDto>(update),
                Message = "Invitation updated",
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