using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.PostUser;
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
public class PostUserController(
        ITokenService tokenService,
        IUserService userService,
        IPostUserService postUserService,
        IConfiguration configuration,
        ICategoryService categoryService,
        IMapper mapper
    ) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<PostUserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<ActionResult> Post(CreatePostUserDto dto) 
    {
        try
        {
            string? id = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (id == null) return Unauthorized();

            UserEntity? user = await userService.GetUserBySid(id);
            if (user == null)
            {
                return StatusCode(404, new ResponseHttp<PostUserDto?>
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

            CategoryEntity? category = await categoryService.GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Category not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (!category.IsActive) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Category is disabled",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            PostUserEntity newPost = await postUserService.Create(dto, user.Id);

            PostUserDto postMapped = mapper.Map<PostUserDto>(newPost);

            return StatusCode(StatusCodes.Status201Created,new ResponseHttp<PostUserDto>
            {
                Data = postMapped,
                Code = StatusCodes.Status201Created,
                Message = "Post created with successfully.",
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<PostUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            PostUserEntity? post = await postUserService.GetById(id);
            if (post == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Post not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            PostUserDto mapped = mapper.Map<PostUserDto>(post);

            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<PostUserDto>
            {
                Data = mapped,
                Code = StatusCodes.Status200OK,
                Message = "Post found with successfully.",
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
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            PostUserEntity? post = await postUserService.GetById(id);
            if (post == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Post not found",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            if (post.UserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseHttp<object>
                {
                    Code = StatusCodes.Status403Forbidden,
                    Message = "This post is not yours",
                    Data = null,
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }
            
            await postUserService.Delete(post);
            
            return StatusCode(StatusCodes.Status204NoContent,new ResponseHttp<object>
            {
                Data = null,
                Code = StatusCodes.Status204NoContent,
                Message = "Post deleted with successfully.",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<PostUserDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] PostUserFilterParam filter)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            var query = postUserService.Query();
            IQueryable<PostUserEntity> appliedFilter = PostUserFilterQuery.ApplyFilter(query, filter);
            
            PaginatedList<PostUserEntity> paginatedList = await PaginatedList<PostUserEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<PostUserDto> dtos = mapper.Map<Page<PostUserDto>>(paginatedList);
        
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