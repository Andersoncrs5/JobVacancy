using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryEntity?> GetByIdAsync(string id);
    Task DeleteAsync(CategoryEntity category);
    Task<CategoryEntity?> CreateAsync(CategoryEntity category);
    Task<CategoryEntity> UpdateAsync(CategoryEntity category, UpdateCategoryDto dto);
    IQueryable GetIQueryable();
    Task<bool> ExistsByName(string name);
    Task<bool> ExistsById(string id);
}