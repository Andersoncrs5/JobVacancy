using AutoMapper;
using JobVacancy.API.models.dtos.Category;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class CategoryService(IUnitOfWork uow, IMapper mapper): ICategoryService
{
    public async Task<CategoryEntity?> GetByIdAsync(string id)
    {
        return await uow.CategoryRepository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(CategoryEntity category)
    {
        uow.CategoryRepository.Delete(category);
        await uow.Commit();
    }

    public async Task<CategoryEntity> CreateAsync(CategoryEntity category)
    {
        CategoryEntity categoryCreated = await uow.CategoryRepository.AddAsync(category); 
        await uow.Commit();
        return categoryCreated;
    }

    public async Task<CategoryEntity> UpdateAsync(CategoryEntity category, UpdateCategoryDto dto)
    {
        mapper.Map(dto, category);

        CategoryEntity update = await uow.CategoryRepository.Update(category);
        await uow.Commit();
        return update;
    }

    public  IQueryable<CategoryEntity> GetIQueryable()
    {
        return uow.CategoryRepository.ReturnIQueryable();
    }

    public async Task<bool> ExistsByName(string name)
    {
        return await uow.CategoryRepository.ExistsByName(name);
    }

    public async Task<bool> ExistsById(string id)
    {
        return await uow.CategoryRepository.ExistsById(id);
    }
    
}