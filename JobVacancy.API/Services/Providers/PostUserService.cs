using AutoMapper;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class PostUserService(IUnitOfWork uow, IMapper mapper): IPostUserService
{
    public async Task<PostUserEntity?> GetById(string id) 
    {
        return await uow.PostUserRepository.GetByIdAsync(id);
    }

    public async Task<bool> ExistsById(string id) 
    {
        return await uow.PostUserRepository.ExistsById(id);
    }

    public async Task Delete(PostUserEntity entity) 
    {
        uow.PostUserRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<PostUserEntity> Create(CreatePostUserDto dto, string userId) 
    {
        PostUserEntity map = mapper.Map<PostUserEntity>(dto);
        map.UserId = userId;
        PostUserEntity newPost = await uow.PostUserRepository.AddAsync(map);
        await uow.Commit();
        return newPost;
    }
    
    public IQueryable<PostUserEntity> Query() 
    {
        return uow.PostUserRepository.ReturnIQueryable();
    }

    public async Task<PostUserEntity> Update(UpdatePostUserDto dto, PostUserEntity postUser) 
    {
        mapper.Map(dto, postUser);
        PostUserEntity update = await uow.PostUserRepository.Update(postUser);
        await uow.Commit();
        return update;
    }
    
}