using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.PostEnterprise;
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
public class PostEnterpriseController(
        ITokenService tokenService,
        IUserService userService,
        IEnterpriseService enterpriseService,
        IPostEnterpriseService postEnterpriseService,
        IConfiguration configuration,
        ICategoryService categoryService,
        IMapper mapper
    ) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<PostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<ActionResult> Post(CreatePostEnterpriseDto dto) 
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            UserEntity? user = await userService.GetUserBySid(userId);
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

            EnterpriseEntity? enter = await enterpriseService.GetByUserId(userId);
            if (enter == null)
            {
                return StatusCode(404, new ResponseHttp<object>
                {
                    Code = 404,
                    Message = "Enterprise not found",
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
            
            PostEnterpriseEntity newPost = await postEnterpriseService.Create(dto, enter.Id);

            PostEnterpriseDto postMapped = mapper.Map<PostEnterpriseDto>(newPost);

            return StatusCode(StatusCodes.Status201Created,new ResponseHttp<PostEnterpriseDto>
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<PostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            PostEnterpriseEntity? post = await postEnterpriseService.GetById(id);
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

            PostEnterpriseDto mapped = mapper.Map<PostEnterpriseDto>(post);

            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<PostEnterpriseDto>
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Del(string id)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            PostEnterpriseEntity? post = await postEnterpriseService.GetById(id);
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

            await postEnterpriseService.Delete(post);

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<PostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] PostEnterpriseFilterParam filter)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            var query = postEnterpriseService.Query();
            IQueryable<PostEnterpriseEntity> appliedFilter = PostEnterpriseFilterQuery.ApplyFilter(query, filter);
            
            PaginatedList<PostEnterpriseEntity> paginatedList = await PaginatedList<PostEnterpriseEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<PostEnterpriseDto> dtos = mapper.Map<Page<PostEnterpriseDto>>(paginatedList);
        
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
    
    
    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<PostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "ENTERPRISE_ROLE")]
    public async Task<IActionResult> Patch(string id, [FromBody] UpdatePostEnterpriseDto dto)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            PostEnterpriseEntity? post = await postEnterpriseService.GetById(id);
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

            PostEnterpriseEntity updated = await postEnterpriseService.Update(dto, post);

            PostEnterpriseDto map = mapper.Map<PostEnterpriseDto>(updated);

            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<PostEnterpriseDto>
            {
                Data = map,
                Code = StatusCodes.Status200OK,
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

}