using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class VacancySkillRepository(AppDbContext context, IRedisService redisService): GenericRepository<VacancySkillEntity>(context, redisService), IVacancySkillRepository
{
    public async Task<bool> ExistsByVacancyIdAndSkillId(string vacancyId, string skillId) 
        => await context.VacancySkillEntities.AnyAsync(x => x.VacancyId == vacancyId && x.SkillId == skillId);
    
    public async Task<VacancySkillEntity?> GetByVacancyIdAndSkillId(string vacancyId, string skillId) 
        => await context.VacancySkillEntities.FirstOrDefaultAsync(x => x.VacancyId == vacancyId && x.SkillId == skillId);
    
    
    
}