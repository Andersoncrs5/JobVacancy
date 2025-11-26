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
public class StorageController(
    IUserService userService,
    IMiniOService miniOService,
    ILogger<ResumeController> logger
) : Controller
{
    [HttpGet("{bucket}/{fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<FileStreamResult>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> DownloadFile(string bucket, string fileName)
    {
        bool existsBucket = await miniOService.ExistsBucket(bucket);
        if (!existsBucket) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Bucket not found",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        bool existsFile = await miniOService.ExistsFile(bucket, fileName);
        if (!existsBucket) 
        {
            return NotFound(new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "File not found",
                Status = false,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        return await miniOService.DownloadFile(bucket, fileName);
    }
}