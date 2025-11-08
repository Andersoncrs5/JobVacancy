using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class EmployeeEnterpriseRepository(AppDbContext context): GenericRepository<EmployeeEnterpriseEntity>(context), IEmployeeEnterpriseRepository
{
    public async Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId)
    {
        return await context.EmployeeEnterprises.AnyAsync(x => x.UserId == userId && x.EnterpriseId == enterpriseId);
    }
}