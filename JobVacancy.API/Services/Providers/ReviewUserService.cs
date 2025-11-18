using JobVacancy.API.models.dtos.ReviewUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class ReviewUserService(IUnitOfWork uow): IReviewUserService
{
    public async Task<bool> ExistsById(string id)
        => await uow.ReviewUserRepository.ExistsById(id);

    public async Task<ReviewUserEntity?> GetById(string id)
        => await uow.ReviewUserRepository.GetByIdAsync(id);

    public async Task<bool> ExistsByActorIdAndTargetId(string actorId, string targetId)
        => await uow.ReviewUserRepository.ExistsByActorIdAndTargetId(actorId, targetId);
    
    public IQueryable<ReviewUserEntity> Query()
        => uow.ReviewUserRepository.ReturnIQueryable();

    public async Task Delete(ReviewUserEntity review)
    {
        await uow.ReviewUserRepository.DeleteAsync(review);
        await uow.Commit();
    }

    public async Task<ReviewUserEntity> Create(CreateReviewUserDto review, string userId)
    {
        ReviewUserEntity map = uow.Mapper.Map<ReviewUserEntity>(review);
        map.ActorId = userId;

        ReviewUserEntity addAsync = await uow.ReviewUserRepository.AddAsync(map);
        await uow.Commit();
        return addAsync;
    }

    public async Task<ReviewUserEntity> Update(UpdateReviewUserDto dto, ReviewUserEntity review)
    {
        if (!string.IsNullOrWhiteSpace(dto.Title))
            review.Title = dto.Title;
        
        if (!string.IsNullOrWhiteSpace(dto.Content))
            review.Content = dto.Content;
        
        if (dto.RatingOverall.HasValue)
            review.RatingOverall = dto.RatingOverall.Value;
        
        if (dto.RatingCulture.HasValue)
            review.RatingCulture = dto.RatingCulture.Value;

        if (dto.RatingCompensation.HasValue)
            review.RatingCompensation = dto.RatingCompensation.Value;
        
        if (dto.RatingManagement.HasValue)
            review.RatingManagement = dto.RatingManagement.Value;
        
        if (dto.RatingWorkLifeBalance.HasValue)
            review.RatingWorkLifeBalance = dto.RatingWorkLifeBalance.Value;

        if (dto.IsAnonymous.HasValue)
            review.IsAnonymous = dto.IsAnonymous.Value;
        
        ReviewUserEntity update = await uow.ReviewUserRepository.Update(review);
        await uow.Commit();
        return update;
    }
    
}