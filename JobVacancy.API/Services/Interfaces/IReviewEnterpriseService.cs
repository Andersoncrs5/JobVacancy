using JobVacancy.API.models.dtos.ReviewEnterprise;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IReviewEnterpriseService
{
    Task<ReviewEnterpriseEntity?> GetById(string id);

    Task<ReviewEnterpriseEntity> Create(CreateReviewEnterpriseDto dto, string enterpriseId, string userId,
        string positionId);

    Task Delete(ReviewEnterpriseEntity review);
    IQueryable<ReviewEnterpriseEntity> Query();
    Task<ReviewEnterpriseEntity> Update(UpdateReviewEnterpriseDto dto, ReviewEnterpriseEntity review);
}