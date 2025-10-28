using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Repositories.Provider;
using JobVacancy.API.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobVacancy.API.Utils.Uow.Provider;

public class UnitOfWork(
    AppDbContext context, 
    UserManager<UserEntity> userManager, 
    RoleManager<RoleEntity> roleManager
    ) : IUnitOfWork, IDisposable
{
    private UserRepository? _userRepository;
    private RoleRepository? _roleRepository;
    private CategoryRepository? _categoryRepository;
    private IndustryRepository? _industryRepository;

    public IUserRepository UserRepository
        => _userRepository ??= new UserRepository(context, userManager);

    public IRoleRepository RoleRepository
        => _roleRepository ??= new RoleRepository(roleManager);
    
    public ICategoryRepository CategoryRepository
        => _categoryRepository ??= new CategoryRepository(context);
    
    public IIndustryRepository IndustryRepository
        => _industryRepository ??= new IndustryRepository(context);

    public async Task Commit() => await context.SaveChangesAsync();
    
    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
    
    public async Task ExecuteTransactionAsync(Func<Task> operation)
    {
        IDbContextTransaction? transaction = null;

        try
        {
            transaction = await BeginTransactionAsync();

            await operation.Invoke();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            throw; 
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }
        
}