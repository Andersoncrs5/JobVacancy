using AutoMapper;
using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class FavoritePostUserService(IUnitOfWork uow, IMapper mapper): IFavoritePostUserService
{
    public async Task<FavoritePostUserEntity?> GetById(string id)
    {
        return await uow.FavoritePostUserRepository.GetByIdAsync(id);
    }
    public async Task<FavoritePostUserEntity?> GetByUserIdAndPostUserIdWithEntity(string userId, string postUserId)
    {
        return await uow.FavoritePostUserRepository.GetByUserIdAndPostUserIdWithEntity(userId, postUserId);
    }
    
     public async Task<FavoritePostUserEntity?> GetByUserIdAndPostUserId(string userId, string postUserId)
    {
        return await uow.FavoritePostUserRepository.GetByUserIdAndPostUserId(userId, postUserId);
    }
    
    public async Task<bool> ExistsByUserIdAndPostUserId(string userId, string postUserId)
    {
        return await uow.FavoritePostUserRepository.ExistsByUserIdAndPostUserId(userId, postUserId);
    }

    public async Task Delete(FavoritePostUserEntity favoritePostUser)
    {
        uow.FavoritePostUserRepository.Delete(favoritePostUser);
        await uow.Commit();
    }

    public async Task<FavoritePostUserEntity> Update(FavoritePostUserEntity favoritePostUser, UpdateFavoritePostUserDto dto)
    {
        mapper.Map(dto, favoritePostUser);

        FavoritePostUserEntity update = await uow.FavoritePostUserRepository.Update(favoritePostUser);
        await uow.Commit();
        return update;
    }

    public IQueryable<FavoritePostUserEntity> Query()
    {
        return uow.FavoritePostUserRepository.ReturnIQueryable();
    }

    public async Task<FavoritePostUserEntity> Create(string userId, string postUserId)
    {
        FavoritePostUserEntity favo = new FavoritePostUserEntity
        {
            UserId = userId,
            PostUserId = postUserId
        };

        FavoritePostUserEntity saved = await uow.FavoritePostUserRepository.AddAsync(favo);
        await uow.Commit();
        
        return saved;
    }
    
}