using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.FavoritePostUser;
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
public class FavoritePostUserController(ITokenService tokenService,
    IUserService userService,
    IPostUserService postUserService,
    IFavoritePostUserService favoritePostUserService,
    IMapper mapper
) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<FavoritePostUserDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] FavoritePostUserFilterParam filter)
    {
        try
        {
            IQueryable<FavoritePostUserEntity> query = favoritePostUserService.Query();
            IQueryable<FavoritePostUserEntity> appliedFilter = FavoritePostUserFilterQuery.ApplyFilter(query, filter);
            
            PaginatedList<FavoritePostUserEntity> paginatedList = await PaginatedList<FavoritePostUserEntity>.CreateAsync(
                source: appliedFilter,
                pageSize: filter.PageSize,
                pageIndex: filter.PageNumber
            );
        
            Page<FavoritePostUserDto> dtos = mapper.Map<Page<FavoritePostUserDto>>(paginatedList);
        
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
    
    [HttpGet("{postId:required}/exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Exists(string postId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
            bool favor = await favoritePostUserService.ExistsByUserIdAndPostUserId(userId, postId); 

            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<bool>
            {
                Data = favor,
                Code = StatusCodes.Status200OK,
                Message = favor ? "Post already is marked with favorite!": "Post is not marked with favorite!",
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<FavoritePostUserDto>))]
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

            if (!await postUserService.ExistsById(postId))
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

            FavoritePostUserEntity? favor = await favoritePostUserService.GetByUserIdAndPostUserId(user.Id, postId);
            if (favor != null)
            {
                await favoritePostUserService.Delete(favor);
                
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

            FavoritePostUserEntity favorSaved = await favoritePostUserService.Create(userId, postId);

            return StatusCode(StatusCodes.Status201Created,new ResponseHttp<FavoritePostUserDto>
            {
                Data = mapper.Map<FavoritePostUserDto>(favorSaved),
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

    [HttpPatch("{id:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<FavoritePostUserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    [Authorize(Roles = "USER_ROLE")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateFavoritePostUserDto dto)
    {
        try
        {
            FavoritePostUserEntity? favor = await favoritePostUserService.GetById(id);
            if (favor == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseHttp<object>
                {
                    Data = null,
                    Code = StatusCodes.Status404NotFound,
                    Message = "Data not found",
                    Status = false,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            FavoritePostUserEntity updated = await favoritePostUserService.Update(favor, dto);

            FavoritePostUserDto map = mapper.Map<FavoritePostUserDto>(updated);
            
            return StatusCode(StatusCodes.Status200OK,new ResponseHttp<FavoritePostUserDto>
            {
                Data = map,
                Code = StatusCodes.Status200OK,
                Message = "Data updated with successfully.",
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