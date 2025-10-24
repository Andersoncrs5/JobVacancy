using Api.Context;
using Api.models.entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Provider;
using Api.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Utils.Uow.Provider;

public class UnitOfWork(
    AppDbContext context, 
    UserManager<UserEntity> userManager, 
    RoleManager<RoleEntity> roleManager
    ) : IUnitOfWork, IDisposable
{
    private UserRepository? _userRepository;

    public IUserRepository UserRepository
        => _userRepository ??= new UserRepository(context, userManager);

    public async Task Commit() => await context.SaveChangesAsync();

    public void Dispose() => context.Dispose();
        
}