using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.CommentPostEnterprise;
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
public class CommentPostEnterpriseController(
    ITokenService tokenService,
    IUserService userService,
    IPostEnterpriseService postEnterpriseService,
    ICommentPostEnterpriseService commentPostEnterpriseService,
    IConfiguration configuration,
    ICategoryService categoryService,
    IMapper mapper
) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<CommentPostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create([FromBody] CreateCommentPostEnterpriseDto dto, [FromQuery] string? parentId )
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            UserEntity? user = await userService.GetUserBySid(userId);
            if (user == null)
            {
                return StatusCode(404, new ResponseHttp<object?>
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

            bool existsPost = await postEnterpriseService.ExistsById(dto.PostId);
            if (!existsPost)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Post not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (!string.IsNullOrEmpty(parentId))
            {
                bool existsComment = await commentPostEnterpriseService.ExistsById(parentId);
                if (!existsComment)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                    {
                        Code = 404,
                        Message = "Comment not found",
                        Data = null,
                        Status = false,
                        Timestamp = DateTimeOffset.UtcNow,
                        TraceId = HttpContext.TraceIdentifier,
                        Version = 1
                    });
                }
            }

            CommentPostEnterpriseEntity commentCreated = await commentPostEnterpriseService.Create(dto, user.Id, parentId);

            CommentPostEnterpriseDto commentMapped = mapper.Map<CommentPostEnterpriseDto>(commentCreated);

            return StatusCode(StatusCodes.Status201Created,new ResponseHttp<CommentPostEnterpriseDto>
            {
                Data = commentMapped,
                Code = StatusCodes.Status201Created,
                Message = "Comment created with successfully.",
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

    [HttpGet("{commentId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<CommentPostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string commentId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            CommentPostEnterpriseEntity? comment = await commentPostEnterpriseService.GetById(commentId);
            if (comment == null)
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Comment not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            CommentPostEnterpriseDto commentMapped = mapper.Map<CommentPostEnterpriseDto>(comment);

            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<CommentPostEnterpriseDto>
            {
                Data = commentMapped,
                Code = StatusCodes.Status200OK,
                Message = "Comment found with successfully.",
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

    [HttpDelete("{commentId:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Del(string commentId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            CommentPostEnterpriseEntity? comment = await commentPostEnterpriseService.GetById(commentId);
            if (comment == null)
            {
                return NotFound(new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Comment not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            await commentPostEnterpriseService.Delete(comment);
            
            return StatusCode(StatusCodes.Status204NoContent,new ResponseHttp<object>
            {
                Data = null,
                Code = StatusCodes.Status204NoContent,
                Message = "Comment found with successfully.",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<CommentPostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get([FromQuery] CommentPostEnterpriseFilterParam filter)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            var query = commentPostEnterpriseService.Query();
            IQueryable<CommentPostEnterpriseEntity> appliedFilter = CommentPostEnterpriseFilterQuery.ApplyFilter(query, filter);
            
            PaginatedList<CommentPostEnterpriseEntity> paginatedList = await PaginatedList<CommentPostEnterpriseEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<CommentPostEnterpriseDto> dtos = mapper.Map<Page<CommentPostEnterpriseDto>>(paginatedList);
        
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
    
}