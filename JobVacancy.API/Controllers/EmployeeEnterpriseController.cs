using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.EmployeeEnterprise;
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
// colocar check para saber se o user ja esta cadastrado na levando em conta a endDate
public class EmployeeEnterpriseController(
    IEnterpriseService enterpriseService,
    IUserService userService,
    IEmployeeInvitationService employeeInvitationService,
    IEmployeeEnterpriseService  employeeEnterpriseService,
    IPositionService positionService,
    IConfiguration configuration,
    IMapper mapper
) : Controller {

    [HttpPost("{invitationId:required}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<EmployeeEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status410Gone, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Create(string invitationId, [FromBody] CreateEmployeeEnterpriseDto dto)
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

            EmployeeInvitationEntity? invitation = await employeeInvitationService.GetById(invitationId);
            if (invitation == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Invitation Not Found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version =  1
                });
            }

            if (invitation.Status != StatusEnum.Accepted)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = "User guest not accepted the invitation",
                    Version = 1,
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
            
            if (invitation.ExpiresAt <= DateTime.UtcNow) 
            {
                return StatusCode(StatusCodes.Status410Gone, new ResponseHttp<object>
                {
                    Version = 1,
                    Message = "Invitation expired",
                    Data = null,
                    Status = false,
                    Code = StatusCodes.Status410Gone,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }

            EmployeeEnterpriseEntity? exists = await employeeEnterpriseService.GetByUserIdAndEnterpriseId(invitation.UserId, enterprise.Id);
            if (exists != null && exists.EndDate == null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = "User already is registered",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            dto.InviteSenderId = invitation.InviteSenderId;
            
            EmployeeEnterpriseEntity map = await employeeEnterpriseService.Create(dto, enterprise.Id,  invitation.UserId);
            
            await employeeInvitationService.Delete(invitation);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<EmployeeEnterpriseDto>
            {
                Code = StatusCodes.Status201Created,
                Data = mapper.Map<EmployeeEnterpriseDto>(map),
                Message = "User registed",
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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EmployeeEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            
            EmployeeEnterpriseEntity? exists = await employeeEnterpriseService.GetById(id);
            if (exists == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Resource not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<EmployeeEnterpriseDto>
            {
                Code = StatusCodes.Status200OK,
                Data = mapper.Map<EmployeeEnterpriseDto>(exists),
                Message = "Resource found",
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            
            EmployeeEnterpriseEntity? exists = await employeeEnterpriseService.GetById(id);
            if (exists == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Resource not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            await employeeEnterpriseService.Delete(exists);
            
            return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
            {
                Code = StatusCodes.Status204NoContent,
                Data = null,
                Message = "User removed",
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
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<EmployeeEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdateEmployeeEnterpriseDto dto) 
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            
            EmployeeEnterpriseEntity? exists = await employeeEnterpriseService.GetById(id);
            if (exists == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Resource not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (
                !string.IsNullOrEmpty(dto.PositionId) && 
                dto.PositionId != exists.PositionId &&
                !await positionService.ExistsById(dto.PositionId)
                ) 
            {
                
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status404NotFound,
                    Message = "Position not found",
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            EmployeeEnterpriseEntity entity = await employeeEnterpriseService.Update(dto, exists);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<object>
            {
                Code = StatusCodes.Status200OK,
                Data = mapper.Map<EmployeeEnterpriseDto>(entity),
                Message = "User removed",
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