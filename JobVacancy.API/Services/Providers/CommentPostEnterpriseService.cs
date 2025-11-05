using AutoMapper;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class CommentPostEnterpriseService(IUnitOfWork uow, IMapper mapper): ICommentPostEnterpriseService
{
    public async Task<CommentPostEnterpriseEntity?> GetById(string id)
    {
        return await uow.CommentPostEnterpriseRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> ExistsById(string id)
    {
        return await uow.CommentPostEnterpriseRepository.ExistsById(id);
    }

    public async Task Delete(CommentPostEnterpriseEntity entity)
    {
        uow.CommentPostEnterpriseRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<CommentPostEnterpriseEntity> Create(CreateCommentPostEnterpriseDto dto, string userId,
        string? parentId)
    {
        CommentPostEnterpriseEntity map = mapper.Map<CommentPostEnterpriseEntity>(dto);
        map.UserId = userId;
        map.ParentCommentId = parentId;

        CommentPostEnterpriseEntity entity = await uow.CommentPostEnterpriseRepository.AddAsync(map);
        await uow.Commit();
        return entity;
    }

    public IQueryable<CommentPostEnterpriseEntity> Query()
    {
        return uow.CommentPostEnterpriseRepository.ReturnIQueryable();
    }

    public async Task<CommentPostEnterpriseEntity> Update(CommentPostEnterpriseEntity comment, UpdateCommentPostEnterpriseDto dto)
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
        
        CommentPostEnterpriseEntity updated = await uow.CommentPostEnterpriseRepository.Update(comment);
        await uow.Commit();
        return updated;
    }
    
}