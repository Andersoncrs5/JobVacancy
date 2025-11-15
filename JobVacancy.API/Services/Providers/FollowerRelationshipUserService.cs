using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.dtos.FollowerRelationshipUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class FollowerRelationshipUserService(IUnitOfWork uow): IFollowerRelationshipUserService
{
    public async Task<bool> ExistsByFollowerIdAndFollowedId(string followerId, string followedId)
        => await uow.FollowerRelationshipUserRepository.ExistsByFollowerIdAndFollowedId(followerId, followedId);
    
    public async Task<FollowerRelationshipUserEntity?> GetByFollowerIdAndFollowedId(string followerId, string followedId)
        => await uow.FollowerRelationshipUserRepository.GetByFollowerIdAndFollowedId(followerId, followedId);

    public IQueryable<FollowerRelationshipUserEntity> Query()
        => uow.FollowerRelationshipUserRepository.ReturnIQueryable();

    public async Task<FollowerRelationshipUserEntity?> GetById(string id)
        => await uow.FollowerRelationshipUserRepository.GetByIdAsync(id);
    
    public async Task Delete(FollowerRelationshipUserEntity followerRelationshipUserEntity)
    {
        uow.FollowerRelationshipUserRepository.Delete(followerRelationshipUserEntity);
        await uow.Commit();
    }

    public async Task<FollowerRelationshipUserEntity> Create(string followerId, string followedId)
    {
        FollowerRelationshipUserEntity relation = new FollowerRelationshipUserEntity
        {
            FollowedId = followedId,
            FollowerId = followerId
        };

        FollowerRelationshipUserEntity addAsync = await uow.FollowerRelationshipUserRepository.AddAsync(relation);
        await uow.Commit();
        return addAsync;
    }

    public async Task<FollowerRelationshipUserEntity> Update(FollowerRelationshipUserEntity relation, UpdateFollowerRelationshipUserDto dto)
    {
        if (dto.WishReceiveNotifyByNewPost.HasValue)
            relation.WishReceiveNotifyByNewPost = dto.WishReceiveNotifyByNewPost.Value;
        
        if (dto.WishReceiveNotifyByNewComment.HasValue)
            relation.WishReceiveNotifyByNewComment = dto.WishReceiveNotifyByNewComment.Value;

        if (dto.WishReceiveNotifyByNewInteraction.HasValue)
            relation.WishReceiveNotifyByNewInteraction = dto.WishReceiveNotifyByNewInteraction.Value;
        
        FollowerRelationshipUserEntity update = await uow.FollowerRelationshipUserRepository.Update(relation);
        await uow.Commit();
        return update;
    }
    
}