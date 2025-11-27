using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.PostUserMedia;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Response;

namespace JobVacancy.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostUserMediaController(
    IConfiguration configuration,
    IUserService userService,
    IMiniOService miniOService,
    IPostUserMediaService postUserMediaService,
    IPostUserService postUserService,
    IMapper mapper,
    ILogger<ResumeController> logger
) : Controller 
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<PostUserMediaDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create([FromForm] CreatePostUserMediaDto dto)
    {
        string bucketName = configuration.GetValue<string>("Buckets:ImageBucketName") ?? throw new ArgumentNullException(nameof(bucketName));
        
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(userId);
        if (user == null) return Unauthorized();
        
        int amount = await postUserMediaService.TotalMediaByPost(dto.PostId);

        if (amount > 15) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Data = null,
                Message = "The number max of medias in post is 15",
                Status = true,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1
            });
        }

        PostUserEntity? post = await postUserService.GetById(dto.PostId);
        if (post is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
            {
                Code = StatusCodes.Status404NotFound,
                Data = null,
                Message = "Post not found",
                Status = true,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1
            });
        }

        if (!post.UserId.Equals(userId)) 
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
            {
                Code = StatusCodes.Status403Forbidden,
                Data = null,
                Message = "You do not have permission to add image this post",
                Status = true,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTimeOffset.UtcNow,
                Version = 1
            });
        }
        
        PutObjectResponse response = await miniOService.UploadOptimized(bucketName, dto.File);

        if (response.Size == 0 || string.IsNullOrWhiteSpace(response.Etag)) 
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

        PostUserMediaEntity media = await postUserMediaService.Create(dto, response, bucketName);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<PostUserMediaDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<PostUserMediaDto>(media),
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            Version = 1,
            TraceId = HttpContext.TraceIdentifier,
            Message = "Image upload with successfully"
        });
        
    }
    
}