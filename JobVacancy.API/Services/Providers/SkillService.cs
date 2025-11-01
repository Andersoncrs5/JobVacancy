using AutoMapper;
using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class SkillService(IUnitOfWork uow, IMapper  mapper): ISkillService
{
    public async Task<SkillEntity?> GetById(string id)
    {
        return await uow.SkillRepository.GetByIdAsync(id);
    }
    
    public async Task Delete(SkillEntity skill)
    {
        uow.SkillRepository.Delete(skill);
        await uow.Commit();
    }

    public async Task<bool> ExistsById(string id)
    {
        return await uow.SkillRepository.ExistsById(id);
    }
    
    public async Task<bool> ExistsByName(string name)
    {
        return await uow.SkillRepository.ExistsByName(name);
    }

    public async Task<SkillEntity> CreateAsync(CreateSkillDto dto)
    {
        SkillEntity map = mapper.Map<SkillEntity>(dto);
        
        SkillEntity created = await uow.SkillRepository.AddAsync(map);
        await uow.Commit();
        return created;
    }

    public async Task<SkillEntity> UpdateAsync(UpdateSkillDto dto, SkillEntity skill)
    {
        mapper.Map(dto, skill);

        SkillEntity update = await uow.SkillRepository.Update(skill);
        await uow.Commit();
        return update;
    }

    public IQueryable<SkillEntity> Query()
    {
        return uow.SkillRepository.ReturnIQueryable();
    }
    
}