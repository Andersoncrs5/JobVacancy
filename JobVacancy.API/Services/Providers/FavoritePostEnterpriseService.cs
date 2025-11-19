using AutoMapper;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class FavoritePostEnterpriseService(IUnitOfWork uow, IMapper mapper): IFavoritePostEnterpriseService
{
    public async Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostIdWithEntity(string userId, string postId)
    {
        return await uow.FavoritePostEnterpriseRepository.GetByUserIdAndPostIdWithEntity(userId, postId);
    }
    
    public async Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostId(string userId, string postId)
    {
        return await uow.FavoritePostEnterpriseRepository.GetByUserIdAndPostId(userId, postId);
    }
    
    public async Task<bool> ExistsByUserIdAndPostId(string userId, string postId)
    {
        return await uow.FavoritePostEnterpriseRepository.ExistsByUserIdAndPostId(userId, postId);
    }
    
    public async Task Delete(FavoritePostEnterpriseEntity favor)
    {
        uow.FavoritePostEnterpriseRepository.Delete(favor);
        await uow.Commit();
    }
    
    public IQueryable<FavoritePostEnterpriseEntity> Query()
    {
        return uow.FavoritePostEnterpriseRepository.Query();
    }
    
    public async Task<FavoritePostEnterpriseEntity> Create(string userId, string postId)
    {
        FavoritePostEnterpriseEntity favo = new FavoritePostEnterpriseEntity
        {
            UserId = userId,
            PostEnterpriseId = postId
        };

        FavoritePostEnterpriseEntity saved = await uow.FavoritePostEnterpriseRepository.AddAsync(favo);
        await uow.Commit();
        
        return saved;
    }
    
}