using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IEnterpriseIndustryService
{
    Task<EnterpriseIndustryEntity?> GetByIdAsync(string id);
    Task<EnterpriseIndustryEntity?> GetByIndustryIdAndEnterpriseId(string industryId, string enterpriseId);
    Task<bool> ExistsByIndustryIdAndEnterpriseId(string industryId, string enterpriseId);
    Task<EnterpriseIndustryEntity> CreateAsync(CreateEnterpriseIndustryDto dto);
    void Delete(EnterpriseIndustryEntity entity);
    IQueryable<EnterpriseIndustryEntity> GetQuery();
    Task<int> CheckAmount(string enterpriseId);
    Task<EnterpriseIndustryEntity> UpdateSimple(EnterpriseIndustryEntity entity);
}