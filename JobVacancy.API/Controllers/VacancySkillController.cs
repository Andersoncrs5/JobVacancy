using AutoMapper;
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
public class VacancySkillController(
    IUserService userService,
    IEnterpriseService enterpriseService,
    IVacancyService vacancyService,
    IVacancySkillService vacancySkillService,
    IAreaService areaService,
    IMapper mapper
) : Controller
{
    [HttpGet("{vacancyId:required}/{skillId:required}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    public async Task<IActionResult> ExistsAsync(string vacancyId, string skillId)
    {
        bool exists = await vacancySkillService.ExistsByVacancyIdAndSkillId(vacancyId, skillId);
        
        return Ok(new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = exists,
            Message = "",
            Status = true,
            Version = 1,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier
        });
    }
}