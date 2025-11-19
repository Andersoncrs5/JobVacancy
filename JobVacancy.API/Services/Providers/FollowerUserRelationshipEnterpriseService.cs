using JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class FollowerUserRelationshipEnterpriseService(IUnitOfWork uow): IFollowerUserRelationshipEnterpriseService
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await uow.FollowerUserRelationshipEnterpriseRepository.ExistsByEnterpriseIdAndUserId(enterpriseId, userId);
    
    public async Task<FollowerUserRelationshipEnterpriseEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await uow.FollowerUserRelationshipEnterpriseRepository.GetByEnterpriseIdAndUserId(enterpriseId, userId);

    public async Task<bool> ExistsById(string id) 
        => await uow.FollowerUserRelationshipEnterpriseRepository.ExistsById(id);
    
    public async Task<FollowerUserRelationshipEnterpriseEntity?> GetById(string id) 
        => await uow.FollowerUserRelationshipEnterpriseRepository.GetByIdAsync(id);

    public IQueryable<FollowerUserRelationshipEnterpriseEntity> Query()
        => uow.FollowerUserRelationshipEnterpriseRepository.Query();
    
    public async Task Delete(FollowerUserRelationshipEnterpriseEntity entity)
    {
        uow.FollowerUserRelationshipEnterpriseRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<FollowerUserRelationshipEnterpriseEntity> Create(string enterpriseId, string userId)
    {
        FollowerUserRelationshipEnterpriseEntity entity = new FollowerUserRelationshipEnterpriseEntity()
        {
            EnterpriseId = enterpriseId,
            UserId = userId
        };

        FollowerUserRelationshipEnterpriseEntity added = await uow.FollowerUserRelationshipEnterpriseRepository.AddAsync(entity);
        await uow.Commit();
        return added;
    }

    public async Task<FollowerUserRelationshipEnterpriseEntity> Update(UpdateFollowerUserRelationshipEnterpriseDto dto, FollowerUserRelationshipEnterpriseEntity entity)
    {
        if (dto.WishReceiveNotifyByNewPost.HasValue)
            entity.WishReceiveNotifyByNewPost = dto.WishReceiveNotifyByNewPost.Value;
        
        if (dto.WishReceiveNotifyByNewVacancy.HasValue)
            entity.WishReceiveNotifyByNewVacancy = dto.WishReceiveNotifyByNewVacancy.Value;
        
        if (dto.WishReceiveNotifyByNewComment.HasValue)
            entity.WishReceiveNotifyByNewComment = dto.WishReceiveNotifyByNewComment.Value;
        
        if (dto.WishReceiveNotifyByNewInteraction.HasValue)
            entity.WishReceiveNotifyByNewInteraction = dto.WishReceiveNotifyByNewInteraction.Value;
        
        FollowerUserRelationshipEnterpriseEntity update = await uow.FollowerUserRelationshipEnterpriseRepository.Update(entity);
        await uow.Commit();
        return update;
    }
    
}