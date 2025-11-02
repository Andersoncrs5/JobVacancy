using AutoMapper;
using JobVacancy.API.models.dtos.UserSkill;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class UserSkillService(IUnitOfWork uow, IMapper  mapper): IUserSkillService
{
    public async Task<bool> ExistsByUserIdAndSkillId(string userId, string skillId)
    {
        return await uow.UserSkillRepository.ExistsByUserIdAndSkillId(userId, skillId);
    }
    
    public async Task<UserSkillEntity?> GetById(string id)
    {
        return await uow.UserSkillRepository.GetByIdAsync(id);
    }
    
    public async Task<UserSkillEntity?> GetByUserIdAndSkillId(string userId, string skillId)
    {
        return await uow.UserSkillRepository.GetByUserIdAndSkillId(userId, skillId);
    }

    public async Task Delete(UserSkillEntity userSkill)
    {
        uow.UserSkillRepository.Delete(userSkill);
        await uow.Commit();
    }

    public async Task<UserSkillEntity> CreateAsync(string userId, string skillId)
    {
        UserSkillEntity skill = new UserSkillEntity()
        {
            UserId = userId,
            SkillId = skillId,
        };

        UserSkillEntity newSkill = await uow.UserSkillRepository.AddAsync(skill);
        await uow.Commit();
        return newSkill;
    }

    public async Task<UserSkillEntity> UpdateAsync(UserSkillEntity userSkill, UpdateUserSkillDto dto)
    {
        var level = userSkill.ProficiencyLevel;
        
        mapper.Map(dto, userSkill);

        if (dto.ProficiencyLevel.HasValue)
        {
            userSkill.ProficiencyLevel = dto.ProficiencyLevel.Value;
        }
        else
        {
            userSkill.ProficiencyLevel = level;
        }

        UserSkillEntity updated = await uow.UserSkillRepository.Update(userSkill);
        await uow.Commit();
        return updated;
    }

    public IQueryable<UserSkillEntity> Query()
    {
        return uow.UserSkillRepository.ReturnIQueryable();
    }
    
}