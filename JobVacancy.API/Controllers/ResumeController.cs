
using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.Resume;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.Resume;
using JobVacancy.API.Utils.Page;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ResumeController(
    IConfiguration configuration,
    IUserService userService,
    IMiniOService miniOService,
    IResumeService resumeService,
    IMapper mapper,
    ILogger<ResumeController> logger
) : Controller
{
    
    [HttpPost]
    [Authorize(Roles = "USER_ROLE")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Post([FromForm] CreateResumeDto dto, [FromQuery] string? version = null)
    {
        string bucketName = configuration.GetValue<string>("Buckets:ResumeBucketName") ?? throw new ArgumentNullException(nameof(bucketName));
        
        string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (id == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(id);
        if (user == null) return Unauthorized();
        
        PutObjectResponse response = await miniOService.UploadOptimized(bucketName, dto.File, version);
        
        if (response.Size == 0 || response.Etag == null) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHttp<object>
            {
                Message = "Error the storage file! Try again later.",
                Status = false,
                TraceId = HttpContext.TraceIdentifier,
                Code = StatusCodes.Status500InternalServerError,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1,
                Data = null
            });
        }

        ResumeEntity entity = await resumeService.Create(dto, user.Id, response);

        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<ResumeDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<ResumeDto>(entity),
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            Version = 1,
            TraceId = HttpContext.TraceIdentifier,
            Message = "Resume added"
        });
    }

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<ResumeDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        ResumeEntity? entity = await resumeService.GetById(id);
        if (entity == null) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Resume not found",
                Status = false,
                Data = null,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1,
            });
        }
        
        return Ok(new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = mapper.Map<ResumeDto>(entity),
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            Version = 1,
            TraceId = HttpContext.TraceIdentifier,
            Message = "Resume found",
        });
    }
    
    [HttpDelete("{id:required}")]
    [Authorize(Roles = "USER_ROLE")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Delete(string id)
    {
        string bucketName = configuration.GetValue<string>("Buckets:ResumeBucketName") ?? throw new ArgumentNullException(nameof(bucketName));
        
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(userId);
        if (user == null) return Unauthorized();
        
        ResumeEntity? entity = await resumeService.GetById(id);
        if (entity == null) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Resume not found",
                Status = false,
                Data = null,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1,
            });
        }

        if (entity.userId != userId) return Forbid();
        
        bool existsFile = await miniOService.ExistsFile(bucketName, entity.ObjectKey);
        if (!existsFile) 
        {
            await resumeService.Delete(entity);
            
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Resume not found",
                Status = false,
                Data = null,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1,
            });
        }

        await miniOService.RemoveFile(bucketName, entity.ObjectKey);
        
        await resumeService.Delete(entity);
        
        return Ok(new ResponseHttp<object>
        {
            Code = StatusCodes.Status200OK,
            Data = null,
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            Version = 1,
            TraceId = HttpContext.TraceIdentifier,
            Message = "Resume removed",
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<Page<ResumeDto>>))]
    public async Task<IActionResult> GetAll([FromQuery] ResumeFilterParams filter)
    {
        IQueryable<ResumeEntity> iQueryable = resumeService.Query();
        IQueryable<ResumeEntity> appliedFilter = ResumeFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<ResumeEntity> paginatedList = await PaginatedList<ResumeEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<ResumeDto> dtos = mapper.Map<Page<ResumeDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}
