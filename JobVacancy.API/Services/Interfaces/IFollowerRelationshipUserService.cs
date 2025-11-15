using JobVacancy.API.models.dtos.FollowerRelationshipUser;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IFollowerRelationshipUserService
{
    Task<bool> ExistsByFollowerIdAndFollowedId(string followerId, string followedId);
    Task<FollowerRelationshipUserEntity?> GetByFollowerIdAndFollowedId(string followerId, string followedId);
    IQueryable<FollowerRelationshipUserEntity> Query();
    Task<FollowerRelationshipUserEntity?> GetById(string id);
    Task Delete(FollowerRelationshipUserEntity followerRelationshipUserEntity);
    Task<FollowerRelationshipUserEntity> Create(string followerId, string followedId);

    Task<FollowerRelationshipUserEntity> Update(FollowerRelationshipUserEntity relation,
        UpdateFollowerRelationshipUserDto dto);
}