using AutoMapper;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class CommentPostUserService(IUnitOfWork work, IMapper mapper): ICommentPostUserService
{
    public async Task<CommentPostUserEntity> Create(CreateCommentPostUserDto dto, string userId, string? parentId)
    {
        CommentPostUserEntity comment = mapper.Map<CommentPostUserEntity>(dto);
        comment.UserId = userId;
        comment.ParentCommentId = parentId;

        CommentPostUserEntity created = await work.CommentPostUserRepository.AddAsync(comment);
        await work.Commit();
        return created;
    }
}