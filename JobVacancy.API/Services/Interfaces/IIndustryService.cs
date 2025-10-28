using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IIndustryService
{
    Task<IndustryEntity?> GetByIdAsync(string id);
    Task DeleteAsync(IndustryEntity industry);
    Task<IndustryEntity> UpdateAsync(IndustryEntity industry, UpdateIndustryDto dto);
    Task<IndustryEntity> CreateAsync(CreateIndustryDto dto);
    
    IQueryable<IndustryEntity> GetIQueryable();
    Task<bool> ExistsByName(string name);
    Task<bool> ExistsById(string id);
}