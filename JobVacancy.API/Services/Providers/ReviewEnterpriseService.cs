using JobVacancy.API.models.dtos.ReviewEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class ReviewEnterpriseService(IUnitOfWork uow): IReviewEnterpriseService
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
    {
        return await uow.ReviewEnterpriseRepository.ExistsByUserIdAndEnterpriseId(userId, enterpriseId);
    }
    
    public async Task<ReviewEnterpriseEntity?> GetById(string id)
    {
        return await uow.ReviewEnterpriseRepository.GetByIdAsync(id);
    }

    public async Task<ReviewEnterpriseEntity> Create(CreateReviewEnterpriseDto dto, string enterpriseId, string userId, string positionId)
    {
        ReviewEnterpriseEntity map = uow.Mapper.Map<ReviewEnterpriseEntity>(dto);
        map.UserId =  userId;
        map.EnterpriseId = enterpriseId;
        map.PositionId = positionId;
        
        ReviewEnterpriseEntity newReview = await uow.ReviewEnterpriseRepository.AddAsync(map);
        await uow.Commit();
        
        return newReview;
    }

    public async Task Delete(ReviewEnterpriseEntity review)
    {
        uow.ReviewEnterpriseRepository.Delete(review);
        await uow.Commit();
    }

    public IQueryable<ReviewEnterpriseEntity> Query()
    {
        return uow.ReviewEnterpriseRepository.ReturnIQueryable();
    }

    public async Task<ReviewEnterpriseEntity> Update(UpdateReviewEnterpriseDto dto, ReviewEnterpriseEntity review)
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
        
        ReviewEnterpriseEntity update = await uow.ReviewEnterpriseRepository.Update(review);
        await uow.Commit();
        return update;
    }
    
}