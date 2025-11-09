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
public class EmployeeEnterpriseController(
    IEnterpriseService enterpriseService,
    IUserService userService,
    IEmployeeInvitationService employeeInvitationService,
    IEmployeeEnterpriseService  employeeEnterpriseService,
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
            
            dto.InviteSenderId = invitation.InviteSenderId;
            
            EmployeeEnterpriseEntity map = await employeeEnterpriseService.Create(dto, enterprise.Id,  invitation.UserId);

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
}