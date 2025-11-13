using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.dtos.Vacancy;
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
public class ApplicationVacancyController(
    IUserService userService,
    IVacancyService vacancyService,
    IApplicationVacancyService applicationVacancyService,
    IMapper mapper
) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<ApplicationVacancyDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status410Gone, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateApplicationVacancyDto dto)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(userId);
        if (user == null) 
        {
            return StatusCode(404, new ResponseHttp<object>
            {
                Code = 404,
                Message = "User not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        VacancyEntity? vacancy = await vacancyService.GetById(dto.VacancyId);
        if (vacancy == null) 
        {
            return StatusCode(404, new ResponseHttp<object>
            {
                Code = 404,
                Message = "Vacancy not found",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (vacancy.ApplicationDeadLine < DateTime.UtcNow)
        {
            return StatusCode(StatusCodes.Status410Gone, new ResponseHttp<object>
            {
                Code = StatusCodes.Status410Gone,
                Message = "The vacancy is deadline",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (vacancy.Status != VacancyStatusEnum.Open)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Message = $"Application denied. The vacancy status is currently {vacancy.Status}.",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        bool exists = await applicationVacancyService.ExistsByVacancyIdAndUserId(dto.VacancyId, userId);
        if (exists) 
        {
            return StatusCode(StatusCodes.Status409Conflict, new ResponseHttp<object>
            {
                Code = StatusCodes.Status409Conflict,
                Message = "You already applied to this vacancy",
                Data = null,
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        ApplicationVacancyEntity app = await applicationVacancyService.Create(dto, userId);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<ApplicationVacancyDto>
        {
            Code = StatusCodes.Status201Created,
            Status = true,
            Data = mapper.Map<ApplicationVacancyDto>(app),
            Message = "Application finished",
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1,
        });
    }
    
}