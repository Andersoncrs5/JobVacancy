using JobVacancy.API.models.dtos.EnterpriseFollowsUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EnterpriseFollowsUserService(IUnitOfWork uow): IEnterpriseFollowsUserService
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await uow.EnterpriseFollowsUserRepository.ExistsByEnterpriseIdAndUserId(enterpriseId, userId);
    
    public async Task<EnterpriseFollowsUserEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await uow.EnterpriseFollowsUserRepository.GetByEnterpriseIdAndUserId(enterpriseId, userId);
    
    public async Task<bool> ExistsById(string id)
        => await uow.EnterpriseFollowsUserRepository.ExistsById(id);

    public async Task<EnterpriseFollowsUserEntity?> GetById(string id)
        => await uow.EnterpriseFollowsUserRepository.GetByIdAsync(id);
    
    public IQueryable<EnterpriseFollowsUserEntity> Query()
        => uow.EnterpriseFollowsUserRepository.Query();

    public async Task Delete(EnterpriseFollowsUserEntity entity)
    {
        await uow.EnterpriseFollowsUserRepository.DeleteAsync(entity);
        await uow.Commit();
    }

    public async Task<EnterpriseFollowsUserEntity> Create(string enterpriseId, string userId)
    {
        EnterpriseFollowsUserEntity follow = new EnterpriseFollowsUserEntity()
        {
            EnterpriseId = enterpriseId,
            UserId = userId
        };

        EnterpriseFollowsUserEntity added = await uow.EnterpriseFollowsUserRepository.AddAsync(follow);
        await uow.Commit();
        return added;
    }

    public async Task<EnterpriseFollowsUserEntity> Update(
        UpdateEnterpriseFollowsUserDto dto, EnterpriseFollowsUserEntity entity
        )
    {
        if (dto.WishReceiveNotifyByNewEndorsement.HasValue)
            entity.WishReceiveNotifyByNewEndorsement = dto.WishReceiveNotifyByNewEndorsement.Value;
        
        if (dto.WishReceiveNotifyByNewPost.HasValue)
            entity.WishReceiveNotifyByNewPost = dto.WishReceiveNotifyByNewPost.Value;
        
        if (dto.WishReceiveNotifyByProfileUpdate.HasValue)
            entity.WishReceiveNotifyByProfileUpdate = dto.WishReceiveNotifyByProfileUpdate.Value;
        
        if (dto.WishReceiveNotifyByNewInteraction.HasValue)
            entity.WishReceiveNotifyByNewInteraction = dto.WishReceiveNotifyByNewInteraction.Value;
        
        EnterpriseFollowsUserEntity update = await uow.EnterpriseFollowsUserRepository.Update(entity);
        await uow.Commit();
        return update;
    }
    
}