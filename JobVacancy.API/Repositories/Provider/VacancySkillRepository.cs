using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class VacancySkillRepository(AppDbContext context): GenericRepository<VacancySkillEntity>(context), IVacancySkillRepository
{
    public async Task<bool> ExistsByVacancyIdAndSkillId(string vacancyId, string skillId) 
        => await context.VacancySkillEntities.AnyAsync(x => x.VacancyId == vacancyId && x.SkillId == skillId);
    
    public async Task<VacancySkillEntity?> GetByVacancyIdAndSkillId(string vacancyId, string skillId) 
        => await context.VacancySkillEntities.FirstOrDefaultAsync(x => x.VacancyId == vacancyId && x.SkillId == skillId);
    
    
    
}