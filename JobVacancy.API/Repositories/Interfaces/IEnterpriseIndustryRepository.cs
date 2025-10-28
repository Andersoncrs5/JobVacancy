using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IEnterpriseIndustryRepository: IGenericRepository<EnterpriseIndustryEntity>
{
    Task<bool> ExistsByIndustryIdAndEnterpriseId(string industryId, string enterpriseId);
    Task<EnterpriseIndustryEntity?> GetByIndustryIdAndEnterpriseId(string industryId, string enterpriseId);
}