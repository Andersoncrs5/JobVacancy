using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Repositories.Provider;
using JobVacancy.API.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.Utils.Uow.Provider;

public class UnitOfWork(
    AppDbContext context, 
    UserManager<UserEntity> userManager, 
    RoleManager<RoleEntity> roleManager
    ) : IUnitOfWork, IDisposable
{
    private UserRepository? _userRepository;
    private RoleRepository? _roleRepository;

    public IUserRepository UserRepository
        => _userRepository ??= new UserRepository(context, userManager);

    public IRoleRepository RoleRepository
        => _roleRepository ??= new RoleRepository(roleManager);

    public async Task Commit() => await context.SaveChangesAsync();

    public void Dispose() => context.Dispose();
        
}