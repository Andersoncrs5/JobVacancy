using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobVacancy.API.Utils.Uow.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IIndustryRepository IndustryRepository { get; }
    IEnterpriseRepository EnterpriseRepository { get; }
    IEnterpriseIndustryRepository EnterpriseIndustryRepository { get; }
    IPostUserRepository PostUserRepository { get; }
    

    Task Commit();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task ExecuteTransactionAsync(Func<Task> operation);
}