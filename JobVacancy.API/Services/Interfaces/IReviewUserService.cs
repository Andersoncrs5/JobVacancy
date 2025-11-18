using JobVacancy.API.models.dtos.ReviewUser;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IReviewUserService
{
    Task<bool> ExistsById(string id);
    Task<ReviewUserEntity?> GetById(string id);
    Task<bool> ExistsByActorIdAndTargetId(string actorId, string targetId);
    IQueryable<ReviewUserEntity> Query();
    Task Delete(ReviewUserEntity review);
    Task<ReviewUserEntity> Create(CreateReviewUserDto review, string userId);
    Task<ReviewUserEntity> Update(UpdateReviewUserDto dto, ReviewUserEntity review);
}