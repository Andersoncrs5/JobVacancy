using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class VacancyService(IUnitOfWork uow): IVacancyService
{
    public async Task<VacancyEntity?> GetById(string id)
        => await uow.VacancyRepository.GetByIdAsync(id);

    public async Task<bool> ExistsById(string id)
        => await uow.VacancyRepository.ExistsById(id);

    public async Task Delete(VacancyEntity entity)
    {
        uow.VacancyRepository.Delete(entity);
        await uow.Commit();
    }

    public IQueryable<VacancyEntity> Query()
        => uow.VacancyRepository.Query();
    
    public async Task<VacancyEntity> Create(CreateVacancyDto dto, string enterpriseId)
    {
        VacancyEntity map = uow.Mapper.Map<VacancyEntity>(dto);
        map.EnterpriseId = enterpriseId;
        map.Status = VacancyStatusEnum.Paused;

        VacancyEntity vacancy = await uow.VacancyRepository.AddAsync(map);
        await uow.Commit();
        
        return vacancy;
    }

    public async Task<VacancyEntity> Update(UpdateVacancyDto dto, VacancyEntity vacancy)
    {
        if (!string.IsNullOrWhiteSpace(dto.Title))
            vacancy.Title = dto.Title;
        
        if (!string.IsNullOrWhiteSpace(dto.Description))
            vacancy.Description = dto.Description;
        
        if (!string.IsNullOrWhiteSpace(dto.Requirements))
            vacancy.Requirements = dto.Requirements;

        if (!string.IsNullOrWhiteSpace(dto.Responsibilities))
            vacancy.Responsibilities = dto.Responsibilities;

        if (!string.IsNullOrWhiteSpace(dto.Benefits))
            vacancy.Benefits = dto.Benefits;

        if (!string.IsNullOrWhiteSpace(dto.AreaId))
            vacancy.AreaId = dto.AreaId;

        if (dto.EmploymentType.HasValue)
            vacancy.EmploymentType = dto.EmploymentType.Value;
        
        if (dto.ExperienceLevel.HasValue)
            vacancy.ExperienceLevel = dto.ExperienceLevel.Value;
        
        if (dto.EducationLevel.HasValue)
            vacancy.EducationLevel = dto.EducationLevel.Value;
        
        if (dto.WorkplaceType.HasValue)
            vacancy.WorkplaceType = dto.WorkplaceType.Value;
        
        if (dto.Currency.HasValue)
            vacancy.Currency = dto.Currency.Value;
        
        if (dto.Status.HasValue)
            vacancy.Status = dto.Status.Value;
        
        if (dto.Seniority.HasValue)
            vacancy.Seniority = dto.Seniority.Value;
        
        if (dto.Opening.HasValue)
            vacancy.Opening = dto.Opening.Value;
        
        if (dto.SalaryMin.HasValue)
            vacancy.SalaryMin = dto.SalaryMin.Value;
        
        if (dto.SalaryMax.HasValue)
            vacancy.SalaryMax = dto.SalaryMax.Value;
        
        if (dto.ApplicationDeadLine.HasValue)
            vacancy.ApplicationDeadLine = dto.ApplicationDeadLine.Value;
        
        VacancyEntity update = await uow.VacancyRepository.Update(vacancy);
        
        await uow.Commit();
        return update;
    }

}