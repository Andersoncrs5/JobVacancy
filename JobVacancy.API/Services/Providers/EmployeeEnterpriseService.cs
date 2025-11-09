using JobVacancy.API.models.dtos.EmployeeEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class EmployeeEnterpriseService(IUnitOfWork uow): IEmployeeEnterpriseService
{
    public async Task<EmployeeEnterpriseEntity?> GetById(string id)
    {
        return await uow.EmployeeEnterpriseRepository.GetByIdAsync(id);
    }

    public async Task<bool> ExistsById(string id)
    {
        return await uow.EmployeeEnterpriseRepository.ExistsById(id);
    }

    public async Task Delete(EmployeeEnterpriseEntity entity)
    {
        uow.EmployeeEnterpriseRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<EmployeeEnterpriseEntity> Create(CreateEmployeeEnterpriseDto dto, string enterpriseId, string invitationUserId)
    {
        EmployeeEnterpriseEntity map = uow.Mapper.Map<EmployeeEnterpriseEntity>(dto);
        
        map.EnterpriseId = enterpriseId;
        map.UserId = invitationUserId;
        
        EmployeeEnterpriseEntity entity = await uow.EmployeeEnterpriseRepository.AddAsync(map);
        await uow.Commit();
        
        return entity;
    }

    public async Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId)
    {
        return await uow.EmployeeEnterpriseRepository.ExistsByUserIdAndEnterpriseId(userId, enterpriseId);
    }

    public async Task<EmployeeEnterpriseEntity> Update(UpdateEmployeeEnterpriseDto dto, EmployeeEnterpriseEntity entity)
    {
        if (!string.IsNullOrWhiteSpace(dto.ContractLink))
        {
            entity.ContractLink = dto.ContractLink;
        }
        
        if (!string.IsNullOrWhiteSpace(dto.SalaryRange))
        {
            entity.SalaryRange = dto.SalaryRange;
        }
        
        if (!string.IsNullOrWhiteSpace(dto.TerminationReason))
        {
            entity.TerminationReason = dto.TerminationReason;
        }
        
        if (!string.IsNullOrWhiteSpace(dto.Notes))
        {
            entity.Notes = dto.Notes;
        }
        
        if (dto.SalaryValue.HasValue)
        {
            entity.SalaryValue = dto.SalaryValue.Value;
        }
        
        if (dto.PaymentFrequency.HasValue)
        {
            entity.PaymentFrequency = dto.PaymentFrequency.Value;
        }
        
        if (dto.ContractLegalType.HasValue)
        {
            entity.ContractLegalType = dto.ContractLegalType.Value;
        }
        
        if (dto.ContractType.HasValue)
        {
            entity.ContractType = dto.ContractType.Value;
        }
        
        if (dto.SalaryCurrency.HasValue)
        {
            entity.SalaryCurrency = dto.SalaryCurrency.Value;
        }
        
        if (dto.EmploymentType.HasValue)
        {
            entity.EmploymentType = dto.EmploymentType.Value;
        }
        
        if (dto.EmploymentStatus.HasValue)
        {
            entity.EmploymentStatus = dto.EmploymentStatus.Value;
        }
        
        if (dto.Currency.HasValue)
        {
            entity.Currency = dto.Currency.Value;
        }
        
        if (dto.StartDate.HasValue)
        {
            entity.StartDate = dto.StartDate.Value;
        }
        
        if (dto.EndDate.HasValue)
        {
            entity.EndDate = dto.EndDate.Value;
        }
        
        EmployeeEnterpriseEntity update = await uow.EmployeeEnterpriseRepository.Update(entity);
        await uow.Commit();
        return entity;
    }

    public IQueryable<EmployeeEnterpriseEntity> Query()
    {
        return uow.EmployeeEnterpriseRepository.ReturnIQueryable();
    }
    
}