using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class SkillRepository(AppDbContext context) 
    : GenericRepository<SkillEntity>(context), ISkillRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await context.Skill.AnyAsync(s => s.Name == name);
    }
}