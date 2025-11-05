using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.entities;
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
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseHttp<object>))]
    public async Task<IActionResult> Toggle(string commentId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null) return Unauthorized();
            
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
    
}