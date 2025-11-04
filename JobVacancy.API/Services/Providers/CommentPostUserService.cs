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

    public async Task<CommentPostUserEntity?> GetById(string id)
    {
        return await work.CommentPostUserRepository.GetByIdAsync(id);
    }

    public async Task Delete(CommentPostUserEntity comment)
    {
        work.CommentPostUserRepository.Delete(comment);
        await work.Commit();
    }

    public async Task<bool> ExistsById(string id)
    {
        return await work.CommentPostUserRepository.ExistsById(id);
    }

    public IQueryable<CommentPostUserEntity> Query()
    {
        return work.CommentPostUserRepository.ReturnIQueryable();
    }

    public async Task<CommentPostUserEntity> Update(CommentPostUserEntity comment, UpdateCommentPostUserDto dto)
    {
        bool isActive = comment.IsActive;
        
        mapper.Map(dto, comment);

        if (dto.IsActive != null)
        {
            comment.IsActive = dto.IsActive.Value;
        }
        else
        {
            comment.IsActive = isActive;
        }
        
        CommentPostUserEntity updated = await work.CommentPostUserRepository.Update(comment);
        await work.Commit();
        return updated;
    }
    
}