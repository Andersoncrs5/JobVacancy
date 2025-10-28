using AutoMapper;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EnterpriseService(IUnitOfWork uow, IMapper mapper): IEnterpriseService
{
    public async Task<EnterpriseEntity?> GetById(string id)
    {
        return await uow.EnterpriseRepository.GetByIdAsync(id);
    }

    public async Task<EnterpriseEntity?> GetByUserId(string userId)
    {
        return await uow.EnterpriseRepository.GetByUserId(userId);
    }

    public void Delete(EnterpriseEntity entity)
    {
        uow.EnterpriseRepository.Delete(entity);
    }

    public async Task<bool> ExistsById(string id)
    {
        return await uow.EnterpriseRepository.ExistsById(id);
    }
    
    public async Task<bool> ExistsByUserId(string userId)
    {
        return await uow.EnterpriseRepository.ExistsByUserId(userId);
    }
    
    public async Task<bool> ExistsByName(string name)
    {
        return await uow.EnterpriseRepository.ExistsByName(name);
    }

    public IQueryable<EnterpriseEntity> Query()
    {
        return uow.EnterpriseRepository.ReturnIQueryable();
    }

    public async Task<EnterpriseEntity> CreateAsync(CreateEnterpriseDto dto, string userId)
    {
        var entity = mapper.Map<EnterpriseEntity>(dto);
        entity.UserId = userId;
        
        EnterpriseEntity enterprise = await uow.EnterpriseRepository.AddAsync(entity);
        await uow.Commit();
        return enterprise;
    }

    public async Task<EnterpriseEntity> UpdateAsync(EnterpriseEntity entity, UpdateEnterpriseDto dto)
    {
        var originalType = entity.Type; 

        mapper.Map(dto, entity);
    
        if (!dto.Type.HasValue) 
        {
            entity.Type = originalType;
        }
        
        EnterpriseEntity enterprise = await uow.EnterpriseRepository.Update(entity);
        await uow.Commit();
        return entity;
    }
    
    
    
}