using AutoMapper;
using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Repositories.Provider;
using JobVacancy.API.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobVacancy.API.Utils.Uow.Provider;

public class UnitOfWork(
    AppDbContext context, 
    UserManager<UserEntity> userManager, 
    RoleManager<RoleEntity> roleManager,
    IMapper mapper
    ) : IUnitOfWork, IDisposable
{
    private UserRepository? _userRepository;
    private RoleRepository? _roleRepository;
    private CategoryRepository? _categoryRepository;
    private IndustryRepository? _industryRepository;
    private EnterpriseRepository? _enterpriseRepository;
    private EnterpriseIndustryRepository? _enterpriseIndustryRepository;
    private PostUserRepository? _postUserRepository;
    private PostEnterpriseRepository? _postEnterpriseRepository;
    private SkillRepository? _skillRepository;
    private UserSkillRepository? _userSkillRepository;
    private FavoritePostUserRepository? _favoritePostUserRepository;
    private FavoritePostEnterpriseRepository? _favoritePostEnterpriseRepository;
    private CommentPostUserRepository? _commentPostUserRepository;
    private FavoriteCommentPostUserRepository? _favoriteCommentPostUserRepository;
    private CommentPostEnterpriseRepository? _commentPostEnterpriseRepository;
    private FavoriteCommentPostEnterpriseRepository? _favoriteCommentPostEnterpriseRepository;
    private EmployeeInvitationRepository? _employeeInvitationRepository;
    private PositionRepository? _positionRepository;
    private EmployeeEnterpriseRepository? _employeeEnterpriseRepository;
    private ReviewEnterpriseRepository? _reviewEnterpriseRepository;
    private IndicationUserRepository? _indicationUserRepository;
    private AreaRepository? _areaRepository;
    private VacancyRepository? _vacancyRepository;
    private VacancySkillRepository? _vacancySkillRepository;
    private ApplicationVacancyRepository? _applicationVacancyRepository;
    private FollowerRelationshipUserRepository? _followerRelationshipUserRepository;
    private FollowerUserRelationshipEnterpriseRepository? _followerUserRelationshipEnterpriseRepository;
    private EnterpriseFollowsUserRepository? _enterpriseFollowsUserRepository;
    private ReviewUserRepository? _reviewUserRepository;
    private UserEvaluationRepository? _userEvaluationRepository;
    public IMapper Mapper { get; } = mapper;
    
    public IUserRepository UserRepository
        => _userRepository ??= new UserRepository(context, userManager);
    public IRoleRepository RoleRepository
        => _roleRepository ??= new RoleRepository(roleManager);
    public ICategoryRepository CategoryRepository
        => _categoryRepository ??= new CategoryRepository(context);
    public IIndustryRepository IndustryRepository
        => _industryRepository ??= new IndustryRepository(context);
    public IEnterpriseRepository EnterpriseRepository
        => _enterpriseRepository ??= new EnterpriseRepository(context);
    public IEnterpriseIndustryRepository EnterpriseIndustryRepository
        => _enterpriseIndustryRepository ??= new EnterpriseIndustryRepository(context);
    public IPostUserRepository PostUserRepository
        => _postUserRepository ??= new PostUserRepository(context);
    public IPostEnterpriseRepository PostEnterpriseRepository
        => _postEnterpriseRepository ??= new PostEnterpriseRepository(context);
    public ISkillRepository SkillRepository
        => _skillRepository ??= new SkillRepository(context);
    public IUserSkillRepository UserSkillRepository
        => _userSkillRepository ??= new UserSkillRepository(context);
    public IFavoritePostUserRepository FavoritePostUserRepository
        => _favoritePostUserRepository ??= new FavoritePostUserRepository(context);
    public IFavoritePostEnterpriseRepository FavoritePostEnterpriseRepository
        => _favoritePostEnterpriseRepository ??= new FavoritePostEnterpriseRepository(context);
    public ICommentPostUserRepository CommentPostUserRepository
        => _commentPostUserRepository ??= new CommentPostUserRepository(context);
    public IFavoriteCommentPostUserRepository FavoriteCommentPostUserRepository
        => _favoriteCommentPostUserRepository ??= new FavoriteCommentPostUserRepository(context);
    public ICommentPostEnterpriseRepository CommentPostEnterpriseRepository
        => _commentPostEnterpriseRepository ??= new CommentPostEnterpriseRepository(context);
    public IFavoriteCommentPostEnterpriseRepository FavoriteCommentPostEnterpriseRepository
        => _favoriteCommentPostEnterpriseRepository ??= new FavoriteCommentPostEnterpriseRepository(context);
    public IEmployeeInvitationRepository EmployeeInvitationRepository
        => _employeeInvitationRepository ??= new EmployeeInvitationRepository(context);
    public IPositionRepository PositionRepository
        => _positionRepository ??= new PositionRepository(context);
    public IEmployeeEnterpriseRepository EmployeeEnterpriseRepository
        => _employeeEnterpriseRepository ??= new EmployeeEnterpriseRepository(context);
    public IReviewEnterpriseRepository ReviewEnterpriseRepository
        => _reviewEnterpriseRepository ??= new ReviewEnterpriseRepository(context);
    public IIndicationUserRepository IndicationUserRepository
        => _indicationUserRepository ??= new IndicationUserRepository(context);
    public IAreaRepository AreaRepository
        => _areaRepository ??= new AreaRepository(context);
    public IVacancyRepository VacancyRepository
        => _vacancyRepository ??= new VacancyRepository(context);
    public IVacancySkillRepository VacancySkillRepository
        =>  _vacancySkillRepository ??= new VacancySkillRepository(context);
    public IApplicationVacancyRepository ApplicationVacancyRepository
        =>  _applicationVacancyRepository ??= new ApplicationVacancyRepository(context);
    public IFollowerRelationshipUserRepository FollowerRelationshipUserRepository
        => _followerRelationshipUserRepository ??= new FollowerRelationshipUserRepository(context);
    public IFollowerUserRelationshipEnterpriseRepository FollowerUserRelationshipEnterpriseRepository
        => _followerUserRelationshipEnterpriseRepository ??= new FollowerUserRelationshipEnterpriseRepository(context);
    public IEnterpriseFollowsUserRepository EnterpriseFollowsUserRepository
        =>  _enterpriseFollowsUserRepository ??= new EnterpriseFollowsUserRepository(context); 
    public IReviewUserRepository ReviewUserRepository
        =>  _reviewUserRepository ??= new ReviewUserRepository(context);
    public IUserEvaluationRepository UserEvaluationRepository
        => _userEvaluationRepository ??= new UserEvaluationRepository(context);
    
    public async Task Commit() 
    {
        try
        {
            await context.SaveChangesAsync();
        } 
        catch (DbUpdateException ex)
        {
            var innerEx = ex.InnerException; 
        
            if (innerEx != null)
            {
                Console.WriteLine($"SQL COMMIT ERROR: {innerEx.Message}");
                throw new InvalidOperationException($"Violação de Integridade do Banco de Dados: {innerEx.Message}", innerEx);
            }

            throw; 
        }
    }
    
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