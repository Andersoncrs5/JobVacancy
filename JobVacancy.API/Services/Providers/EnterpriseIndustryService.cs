using AutoMapper;
using JobVacancy.API.models.dtos.EnterpriseIndustry;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EnterpriseIndustryService(IUnitOfWork uow, IMapper mapper): IEnterpriseIndustryService
{
    public async Task<EnterpriseIndustryEntity?> GetByIdAsync(string id)
    {
        return await uow.EnterpriseIndustryRepository.GetByIdAsync(id);
    }

    public async Task<EnterpriseIndustryEntity?> GetByIndustryIdAndEnterpriseId(string industryId, string enterpriseId)
    {
        return await uow.EnterpriseIndustryRepository.GetByIndustryIdAndEnterpriseId(industryId, enterpriseId);
    }
    
    public async Task<bool> ExistsByIndustryIdAndEnterpriseId(string industryId, string enterpriseId)
    {
        return await uow.EnterpriseIndustryRepository.ExistsByIndustryIdAndEnterpriseId(industryId, enterpriseId);
    }

    public async Task<EnterpriseIndustryEntity> CreateAsync(CreateEnterpriseIndustryDto dto)
    {
        int amount = await CheckAmount(dto.EnterpriseId);
        if (amount > 15)
        {
            throw new Exception("Enterprise industry amount exceed");
        }

        EnterpriseIndustryEntity entity = new EnterpriseIndustryEntity
        {
            EnterpriseId = dto.EnterpriseId,
            IndustryId = dto.IndustryId,
            IsPrimary = dto.IsPrimary,
        };

        EnterpriseIndustryEntity async = await uow.EnterpriseIndustryRepository.AddAsync(entity);
        await uow.Commit();
        return async;
    }

    public async Task<int> CheckAmount(string enterpriseId)
    {
        return await uow.EnterpriseIndustryRepository.GetAmountIndustryByEnterpriseId(enterpriseId);
    }

    public void Delete(EnterpriseIndustryEntity entity)
    {
        uow.EnterpriseIndustryRepository.Delete(entity);
        uow.Commit();
    }

    public IQueryable<EnterpriseIndustryEntity> GetQuery()
    {
        return uow.EnterpriseIndustryRepository.ReturnIQueryable();
    }
    
    public async Task<EnterpriseIndustryEntity> UpdateSimple(EnterpriseIndustryEntity entity)
    {
        EnterpriseIndustryEntity update = await uow.EnterpriseIndustryRepository.Update(entity);
        await uow.Commit();
        return update;
    }
    
}