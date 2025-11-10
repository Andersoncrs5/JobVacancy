using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IReviewEnterpriseRepository: IGenericRepository<ReviewEnterpriseEntity>
{ 
    Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId);
    Task<ReviewEnterpriseEntity?> GetByUserIdAndEnterpriseId(string userId, string enterpriseId);
}