using System.Security.Claims;
using AutoMapper;
using JobVacancy.API.Configs.kafka.Enums;
using JobVacancy.API.models.dtos.UserContentReaction;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Filters.UserContentReaction;
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
public class UserContentReactionController(
    IUserService userService,
    IUserContentReactionService reactionService,
    IKafkaProducerService kafkaProducerService,
    IMapper mapper
    ):Controller
{
    [HttpGet("{contentId:required}/{target:required}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<bool>))]
    public async Task<IActionResult> Exists(string contentId, ReactionTargetEnum target)
    {
        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        bool reaction = await reactionService.ExistsByUserIdAndContentIdAndReaction(userId, contentId, target);
        
        return Ok(new ResponseHttp<bool>
        {
            Code = StatusCodes.Status200OK,
            Data = reaction,
            Message = "",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseHttp<object>))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseHttp<UserContentReactionDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Post([FromBody] CreateUserContentReactionDto dto)
    {
        await kafkaProducerService.MetricSend(
            dto.ContentId,
            ActionEnum.Sum,
            dto.TargetType,
            dto.ReactionType
        );

        string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId == null) return Unauthorized();

        UserEntity? user = await userService.GetUserBySid(userId);
        if (user == null) return Unauthorized();

        UserContentReactionEntity? reaction = await reactionService.GetByUserIdAndContentIdAndReaction(user.Id, dto.ContentId, dto.TargetType);

        if (reaction != null && reaction.ReactionType == dto.ReactionType)
        {
            await reactionService.Delete(reaction);

            return Ok(new ResponseHttp<object>
            {
                Code = StatusCodes.Status200OK,
                Data = null,
                Message = "Reaction removed",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }

        if (reaction != null && reaction.ReactionType != dto.ReactionType)
        {
            reaction.ReactionType = dto.ReactionType;
            UserContentReactionEntity type = await reactionService.Update(reaction);

            return Ok(new ResponseHttp<UserContentReactionDto>
            {
                Code = StatusCodes.Status200OK,
                Data = mapper.Map<UserContentReactionDto>(type),
                Message = "Reaction updated",
                Status = true,
                Timestamp = DateTimeOffset.UtcNow,
                TraceId = HttpContext.TraceIdentifier,
                Version = 1
            });
        }
        
        UserContentReactionEntity saved = await reactionService.Create(userId, dto.ContentId, dto.ReactionType, dto.TargetType);
        
        await kafkaProducerService.MetricSend(
            dto.ContentId,
            ActionEnum.Sum,
            dto.TargetType,
            dto.ReactionType
        );
        
        
        return StatusCode(StatusCodes.Status201Created, new ResponseHttp<UserContentReactionDto>
        {
            Code = StatusCodes.Status201Created,
            Data = mapper.Map<UserContentReactionDto>(saved),
            Message = "Reaction added",
            Status = true,
            Timestamp = DateTimeOffset.UtcNow,
            TraceId = HttpContext.TraceIdentifier,
            Version = 1
        });
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Page<UserContentReactionDto>))]
    public async Task<IActionResult> GetAll([FromQuery] UserContentReactionFilterParams filter)
    {
        IQueryable<UserContentReactionEntity> iQueryable = reactionService.Query();
        IQueryable<UserContentReactionEntity> appliedFilter = UserContentReactionFilterQuery.ApplyFilter(iQueryable, filter);
        PaginatedList<UserContentReactionEntity> paginatedList = await PaginatedList<UserContentReactionEntity>.CreateAsync(
            source: appliedFilter,
            pageSize: filter.PageSize,
            pageIndex: filter.PageNumber
        );
        
        Page<UserContentReactionDto> dtos = mapper.Map<Page<UserContentReactionDto>>(paginatedList);
        
        return Ok(dtos);
    }

    
}