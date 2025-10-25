using JobVacancy.API.models.dtos.Role;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Provider;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;
using JobVacancy.API.Utils.Uow.Provider;
using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.Services.Providers;

public class RolesService(IUnitOfWork uow): IRolesService
{
    private RoleResult ReturnResult(IdentityResult result)
    {
        if (result.Succeeded)
        {
            return new RoleResult
            {
                Succeeded = true,
                User = null,
                Errors = null
            };
        }
        else
        {
            return new RoleResult
            {
                Succeeded = false,
                User = null,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }

    public async Task<RoleEntity?> GetById(string id)
    {
        return await uow.RoleRepository.GetByIdAsync(id);
    }

    public async Task<RoleEntity?> GetByName(string name)
    {
        return await uow.RoleRepository.GetByNameAsync(name);
    }
    
    public async Task<RoleResult> CreateRole(RoleEntity role)
    {
        var result = await uow.RoleRepository.CreateAsync(role);
        if (result.Succeeded)
        {
            await uow.Commit();
        }

        return ReturnResult(result);
    }

    public async Task<IEnumerable<RoleEntity>> GetAllRoles()
    {
        return await uow.RoleRepository.GetAllAsync();
    }
    
}