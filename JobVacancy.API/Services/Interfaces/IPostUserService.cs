using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IPostUserService
{
    Task<PostUserEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task Delete(PostUserEntity entity);
    Task<PostUserEntity> Create(CreatePostUserDto dto, string userId);
    IQueryable<PostUserEntity> Query();
    Task<PostUserEntity> Update(UpdatePostUserDto dto, PostUserEntity postUser);
}