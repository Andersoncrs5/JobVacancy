using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class IndicationUserRepository(AppDbContext context): GenericRepository<IndicationUserEntity>(context), IIndicationUserRepository
{
    public async Task<bool> ExistsByEndorserIdAndEndorsedId(string endorserId, string endorsedId)
    {
        return await context.IndicationUsers.AnyAsync(x => x.EndorserId == endorserId && x.EndorsedId == endorsedId);
    }
    
    public async Task<IndicationUserEntity?> GetByEndorserIdAndEndorsedId(string endorserId, string endorsedId)
    {
        return await context.IndicationUsers.FirstOrDefaultAsync(x => x.EndorserId == endorserId && x.EndorsedId == endorsedId);
    }
    
    
}