using AutoMapper;
using JobVacancy.API.models.dtos.Industry;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class IndustryService(IUnitOfWork uow, IMapper mapper): IIndustryService
{
    public async Task<IndustryEntity?> GetByIdAsync(string id)
    {
        return await uow.IndustryRepository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(IndustryEntity industry)
    {
        uow.IndustryRepository.Delete(industry);
        await uow.Commit();
    }

    public async Task<IndustryEntity> UpdateAsync(IndustryEntity industry, UpdateIndustryDto dto)
    {
        mapper.Map(dto, industry);
        IndustryEntity update = await uow.IndustryRepository.Update(industry);
        await uow.Commit();
        return update;
    }

    public async Task<IndustryEntity> CreateAsync(CreateIndustryDto dto)
    {
        IndustryEntity industry = mapper.Map<IndustryEntity>(dto);

        IndustryEntity entity = await uow.IndustryRepository.AddAsync(industry);
        await uow.Commit();
        return entity;
    }

    public IQueryable<IndustryEntity> GetIQueryable()
    {
        return uow.IndustryRepository.ReturnIQueryable();
    }

    public async Task<bool> ExistsByName(string name)
    {
        return await uow.IndustryRepository.ExistsByName(name);
    }
    
    public async Task<bool> ExistsById(string id)
    {
        return await uow.IndustryRepository.ExistsById(id);
    }
    
}