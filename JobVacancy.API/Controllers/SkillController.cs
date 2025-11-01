using AutoMapper;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
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
public class SkillController(
    ITokenService tokenService,
    ISkillService skillService,
    IRolesService rolesService,
    IConfiguration configuration,
    IMapper mapper
) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<SkillDto>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [EnableRateLimiting("CreateItemPolicy")]
    [Authorize(Roles = "MASTER_ROLE, SUPER_ADM_ROLE")]
    public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
    {
        try
        {
            bool checkName = await skillService.ExistsByName(dto.Name);
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

            SkillEntity newSkill = await skillService.CreateAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<SkillDto>
            {
                Data = mapper.Map<SkillDto>(newSkill),
                Message = "Category created with success.",
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