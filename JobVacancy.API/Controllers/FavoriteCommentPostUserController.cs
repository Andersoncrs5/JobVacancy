using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.FavoriteCommentPostUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.FavoriteCommentPostUser;
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
public class FavoriteCommentPostUserController(
    ITokenService tokenService,
    IUserService userService,
    IPostUserService postUserService,
    ICommentPostUserService  commentPostUserService,
    IFavoriteCommentPostUserService favoriteCommentPostUserService,
    IConfiguration configuration,
    ICategoryService categoryService,
    IMapper mapper
) : Controller {

    [HttpPost("{commentId}/Toggle")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Toggle(string commentId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            if (!await commentPostUserService.ExistsById(commentId))
            {
                return NotFound(new ResponseHttp<object>
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
            
            FavoriteCommentEntity? favorite = await favoriteCommentPostUserService.GetByCommentIdAndUserId(commentId, userId);
            if (favorite != null)
            {
                await favoriteCommentPostUserService.Delete(favorite);
                
                return StatusCode(StatusCodes.Status204NoContent, new ResponseHttp<object>
                {
                    Data = null,
                    Code = 204,
                    Message = "Comment removed",
                    Status = true,
                    Timestamp = DateTimeOffset.UtcNow,
                    TraceId = HttpContext.TraceIdentifier,
                    Version = 1
                });
            }

            await favoriteCommentPostUserService.Create(commentId, userId);
            
            return StatusCode(StatusCodes.Status201Created, new ResponseHttp<object>
            {
                Data = null,
                Code = 201,
                Message = "Comment added",
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

    [HttpGet("{commentId}/Exists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Exists(string commentId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();

            if (!await commentPostUserService.ExistsById(commentId))
            {
                return NotFound(new ResponseHttp<object>
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
            
            bool favorite = await favoriteCommentPostUserService.ExistsByCommentIdAndUserId(commentId, userId);
            
            return StatusCode(StatusCodes.Status200OK, new ResponseHttp<bool>
            {
                Data = favorite,
                Code = 200,
                Message = favorite ? "Comment already is added" : "Comment is not added",
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<FavoriteCommentPostUserDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> GetAll([FromQuery] FavoriteCommentFilter filter )
    {
        IQueryable<FavoriteCommentEntity> query = await favoriteCommentPostUserService.GetAllQuery();
        PaginatedList<FavoriteCommentEntity> paginatedList = await PaginatedList<FavoriteCommentEntity>.CreateAsync(
            source: query,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<FavoriteCommentPostUserDto> dtos = mapper.Map<Page<FavoriteCommentPostUserDto>>(paginatedList);
        
        return Ok(dtos);
    }
    
}