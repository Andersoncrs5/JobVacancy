using Api.Repositories.Interfaces;

namespace Api.Utils.Uow.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }

    Task Commit();
}