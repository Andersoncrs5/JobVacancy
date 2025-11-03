using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.FavoritePostEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.FavoritePostEnterprise;
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
public class FavoritePostEnterpriseController(ITokenService tokenService,
    IUserService userService,
    IPostEnterpriseService postEnterpriseService,
    IFavoritePostEnterpriseService favoritePostEnterpriseService,
    IMapper mapper
) : Controller
{
    
    [HttpGet("{postId:required}/exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Exists(string postId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            bool check = await favoritePostEnterpriseService.ExistsByUserIdAndPostId(userId, postId);

            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
            {
                Data = check,
                Code = StatusCodes.Status200OK,
                Message = check ? "Post already is added" : "Post not added",
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
    
    [HttpPost("{postId:required}/Toggle")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<FavoritePostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Toggle(string postId)
    {
        try
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

            if (!await postEnterpriseService.ExistsById(postId))
            {
                return StatusCode(404, new ResponseHttp<object>
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

            FavoritePostEnterpriseEntity? favor = await favoritePostEnterpriseService.GetByUserIdAndPostId(user.Id, postId);
            if (favor != null)
            {
                await favoritePostEnterpriseService.Delete(favor);
                
                return StatusCode(StatusCodes.Status204NoContent,new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status204NoContent,
                    Message = "Post removed with successfully.",
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            FavoritePostEnterpriseEntity favorSaved = await favoritePostEnterpriseService.Create(userId, postId);

            return StatusCode(StatusCodes.Status201Created,new ResponseHttp<FavoritePostEnterpriseDto>
            {
                Data = mapper.Map<FavoritePostEnterpriseDto>(favorSaved),
                Code = StatusCodes.Status201Created,
                Message = "Post added with successfully.",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<FavoritePostEnterpriseDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] FavoritePostEnterpriseFilterParam filter)
    {
        try
        {
            IQueryable<FavoritePostEnterpriseEntity> query = favoritePostEnterpriseService.Query();
            IQueryable<FavoritePostEnterpriseEntity> appliedFilter = FavoritePostEnterpriseFilterQuery.ApplyFilter(query, filter);
            
            PaginatedList<FavoritePostEnterpriseEntity> paginatedList = await PaginatedList<FavoritePostEnterpriseEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<FavoritePostEnterpriseDto> dtos = mapper.Map<Page<FavoritePostEnterpriseDto>>(paginatedList);
        
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