using AutoMapper;
using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Repositories.Provider;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Services.Providers;
using JobVacancy.API.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobVacancy.API.Utils.Uow.Provider;

public class UnitOfWork(
    AppDbContext context, 
    UserManager<UserEntity> userManager, 
    RoleManager<RoleEntity> roleManager,
    StackExchange.Redis.IDatabase db,
    IRedisService redisService,
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
    private UserContentReactionRepository? _userContentReactionRepository;
    private RedisService? _redisService;
    private PostUserMetricsRepository? _postUserMetricsRepository;
    private ResumeRepository? _resumeRepository;
    private PostUserMediaRepository? _postUserMediaRepository;
    public IMapper Mapper { get; } = mapper;
    
    public IUserRepository UserRepository
        => _userRepository ??= new UserRepository(context, userManager);
    public IRoleRepository RoleRepository
        => _roleRepository ??= new RoleRepository(roleManager);
    public ICategoryRepository CategoryRepository
        => _categoryRepository ??= new CategoryRepository(context, redisService);
    public IIndustryRepository IndustryRepository
        => _industryRepository ??= new IndustryRepository(context, redisService);
    public IEnterpriseRepository EnterpriseRepository
        => _enterpriseRepository ??= new EnterpriseRepository(context, redisService);
    public IEnterpriseIndustryRepository EnterpriseIndustryRepository
        => _enterpriseIndustryRepository ??= new EnterpriseIndustryRepository(context, redisService);
    public IPostUserRepository PostUserRepository
        => _postUserRepository ??= new PostUserRepository(context, redisService);
    public IPostEnterpriseRepository PostEnterpriseRepository
        => _postEnterpriseRepository ??= new PostEnterpriseRepository(context, redisService);
    public ISkillRepository SkillRepository
        => _skillRepository ??= new SkillRepository(context, redisService);
    public IUserSkillRepository UserSkillRepository
        => _userSkillRepository ??= new UserSkillRepository(context, redisService);
    public IFavoritePostUserRepository FavoritePostUserRepository
        => _favoritePostUserRepository ??= new FavoritePostUserRepository(context, redisService);
    public IFavoritePostEnterpriseRepository FavoritePostEnterpriseRepository
        => _favoritePostEnterpriseRepository ??= new FavoritePostEnterpriseRepository(context, redisService);
    public ICommentPostUserRepository CommentPostUserRepository
        => _commentPostUserRepository ??= new CommentPostUserRepository(context, redisService);
    public IFavoriteCommentPostUserRepository FavoriteCommentPostUserRepository
        => _favoriteCommentPostUserRepository ??= new FavoriteCommentPostUserRepository(context, redisService);
    public ICommentPostEnterpriseRepository CommentPostEnterpriseRepository
        => _commentPostEnterpriseRepository ??= new CommentPostEnterpriseRepository(context, redisService);
    public IFavoriteCommentPostEnterpriseRepository FavoriteCommentPostEnterpriseRepository
        => _favoriteCommentPostEnterpriseRepository ??= new FavoriteCommentPostEnterpriseRepository(context, redisService);
    public IEmployeeInvitationRepository EmployeeInvitationRepository
        => _employeeInvitationRepository ??= new EmployeeInvitationRepository(context, redisService);
    public IPositionRepository PositionRepository
        => _positionRepository ??= new PositionRepository(context, redisService);
    public IEmployeeEnterpriseRepository EmployeeEnterpriseRepository
        => _employeeEnterpriseRepository ??= new EmployeeEnterpriseRepository(context, redisService);
    public IReviewEnterpriseRepository ReviewEnterpriseRepository
        => _reviewEnterpriseRepository ??= new ReviewEnterpriseRepository(context, redisService);
    public IIndicationUserRepository IndicationUserRepository
        => _indicationUserRepository ??= new IndicationUserRepository(context, redisService);
    public IAreaRepository AreaRepository
        => _areaRepository ??= new AreaRepository(context, redisService);
    public IVacancyRepository VacancyRepository
        => _vacancyRepository ??= new VacancyRepository(context, redisService);
    public IVacancySkillRepository VacancySkillRepository
        =>  _vacancySkillRepository ??= new VacancySkillRepository(context, redisService);
    public IApplicationVacancyRepository ApplicationVacancyRepository
        =>  _applicationVacancyRepository ??= new ApplicationVacancyRepository(context, redisService);
    public IFollowerRelationshipUserRepository FollowerRelationshipUserRepository
        => _followerRelationshipUserRepository ??= new FollowerRelationshipUserRepository(context, redisService);
    public IFollowerUserRelationshipEnterpriseRepository FollowerUserRelationshipEnterpriseRepository
        => _followerUserRelationshipEnterpriseRepository ??= new FollowerUserRelationshipEnterpriseRepository(context, redisService);
    public IEnterpriseFollowsUserRepository EnterpriseFollowsUserRepository
        =>  _enterpriseFollowsUserRepository ??= new EnterpriseFollowsUserRepository(context, redisService); 
    public IReviewUserRepository ReviewUserRepository
        =>  _reviewUserRepository ??= new ReviewUserRepository(context, redisService);
    public IUserEvaluationRepository UserEvaluationRepository
        => _userEvaluationRepository ??= new UserEvaluationRepository(context, redisService);
    public IUserContentReactionRepository UserContentReactionRepository
        => _userContentReactionRepository ??= new UserContentReactionRepository(context, redisService);
    public IRedisService RedisService
        => _redisService ??= new RedisService(db);
    public IPostUserMetricsRepository PostUserMetricsRepository 
        => _postUserMetricsRepository ??= new PostUserMetricsRepository(context, redisService);
    public IResumeRepository ResumeRepository
        => _resumeRepository ??= new ResumeRepository(context, redisService);
    public IPostUserMediaRepository PostUserMediaRepository
        => _postUserMediaRepository ??= new PostUserMediaRepository(context, redisService);
    
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