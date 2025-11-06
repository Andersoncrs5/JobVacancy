using AutoMapper;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
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
    IPostEnterpriseRepository PostEnterpriseRepository { get; }
    ISkillRepository SkillRepository { get; }
    IUserSkillRepository UserSkillRepository { get; }
    IFavoritePostUserRepository FavoritePostUserRepository { get; }
    IFavoritePostEnterpriseRepository FavoritePostEnterpriseRepository { get; }
    ICommentPostUserRepository CommentPostUserRepository { get; }
    ICommentPostEnterpriseRepository CommentPostEnterpriseRepository { get; }
    IFavoriteCommentPostUserRepository FavoriteCommentPostUserRepository { get; }
    IFavoriteCommentPostEnterpriseRepository FavoriteCommentPostEnterpriseRepository { get; }
    IEmployeeInvitationRepository EmployeeInvitationRepository { get; }
    IMapper Mapper { get; }
    
    Task Commit();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task ExecuteTransactionAsync(Func<Task> operation);
}