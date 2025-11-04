using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.CommentPostUser;
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
public class CommentPostUserController(
    ITokenService tokenService,
    IUserService userService,
    IPostUserService postUserService,
    ICommentPostUserService  commentPostUserService,
    IConfiguration configuration,
    ICategoryService categoryService,
    IMapper mapper
) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<CommentPostUserDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] CommentPostUserFilterParam filter )
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            var query = commentPostUserService.Query();
            IQueryable<CommentPostUserEntity> appliedFilter = CommentPostUserFilterQuery.ApplyFilter(query, filter);
            
            PaginatedList<CommentPostUserEntity> paginatedList = await PaginatedList<CommentPostUserEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<CommentPostUserDto> dtos = mapper.Map<Page<CommentPostUserDto>>(paginatedList);
        
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
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<CommentPostUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Create([FromBody] CreateCommentPostUserDto dto, [FromQuery] string? parentId )
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

            bool existsPost = await postUserService.ExistsById(dto.PostId);
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
                bool existsComment = await commentPostUserService.ExistsById(parentId);
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

            CommentPostUserEntity commentCreated = await commentPostUserService.Create(dto, user.Id, parentId);

            CommentPostUserDto commentMapped = mapper.Map<CommentPostUserDto>(commentCreated);

            return StatusCode(StatusCodes.Status201Created,new ResponseHttp<CommentPostUserDto>
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

    [HttpGet("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<CommentPostUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            CommentPostUserEntity? commentPostUserEntity = await commentPostUserService.GetById(id);
            if (commentPostUserEntity == null)
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
            
            CommentPostUserDto commentMapped = mapper.Map<CommentPostUserDto>(commentPostUserEntity);
            
            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<CommentPostUserDto>
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
    
    [HttpDelete("{id:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Del(string id)
    {
        try
        {
            CommentPostUserEntity? commentPostUserEntity = await commentPostUserService.GetById(id);
            if (commentPostUserEntity == null)
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
            
            await commentPostUserService.Delete(commentPostUserEntity);
            
            return StatusCode(StatusCodes.Status204NoContent,new ResponseHttp<object>
            {
                Data = null,
                Code = StatusCodes.Status204NoContent,
                Message = "Comment deleted with successfully.",
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
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<CommentPostUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Update(string id, UpdateCommentPostUserDto dto)
    {
        try
        {
            CommentPostUserEntity? commentPostUserEntity = await commentPostUserService.GetById(id);
            if (commentPostUserEntity == null)
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

            CommentPostUserEntity update = await commentPostUserService.Update(commentPostUserEntity, dto);

            CommentPostUserDto commentMapped = mapper.Map<CommentPostUserDto>(update);
            
            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<CommentPostUserDto>
            {
                Data = commentMapped,
                Code = StatusCodes.Status200OK,
                Message = "Comment updated with successfully.",
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