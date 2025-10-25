using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Utils.Uow.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }

    Task Commit();
}