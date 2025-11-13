using JobVacancy.API.models.dtos.VacancySkill;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class VacancySkillService(IUnitOfWork uow): IVacancySkillService
{
    public async Task<bool> ExistsByVacancyIdAndSkillId(string vacancyId, string skillId)
        => await uow.VacancySkillRepository.ExistsByVacancyIdAndSkillId(vacancyId, skillId);
    
    public async Task<VacancySkillEntity?> GetByVacancyIdAndSkillId(string vacancyId, string skillId)
        => await uow.VacancySkillRepository.GetByVacancyIdAndSkillId(vacancyId, skillId);
    
    public IQueryable<VacancySkillEntity> Query()
        => uow.VacancySkillRepository.ReturnIQueryable();
    
    public async Task<bool> ExistsById(string id)
        => await uow.VacancySkillRepository.ExistsById(id);

    public async Task<VacancySkillEntity?> GetById(string id)
        => await uow.VacancySkillRepository.GetByIdAsync(id);
    
    public async Task Delete(VacancySkillEntity entity)
    {
        uow.VacancySkillRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<VacancySkillEntity> CreateAsync(CreateVacancySkillDto dto)
    {
        VacancySkillEntity map = uow.Mapper.Map<VacancySkillEntity>(dto);

        VacancySkillEntity skillAdded = await uow.VacancySkillRepository.AddAsync(map);
        
        await uow.Commit();
        return skillAdded;
    }

    public async Task<VacancySkillEntity> UpdateAsync(UpdateVacancyDto dto, VacancySkillEntity entity)
    {
        if (!string.IsNullOrWhiteSpace(dto.SkillId))
            entity.SkillId = dto.SkillId;
        
        if (!string.IsNullOrWhiteSpace(dto.Notes))
            entity.Notes = dto.Notes;
        
        if (dto.RequiredLevel.HasValue)
            entity.RequiredLevel = dto.RequiredLevel.Value;
        
        if (dto.IsMandatory.HasValue)
            entity.IsMandatory = dto.IsMandatory.Value;
        
        if (dto.Weight.HasValue)
            entity.Weight = dto.Weight.Value;
        
        if (dto.YearsOfExperienceRequired.HasValue)
            entity.YearsOfExperienceRequired = dto.YearsOfExperienceRequired.Value;
        
        VacancySkillEntity update = await uow.VacancySkillRepository.Update(entity);
        await uow.Commit();
        return update;
    }
    
}