using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface ICommentPostUserService
{
    Task<CommentPostUserEntity> Create(CreateCommentPostUserDto dto, string userId, string? parentId);
    Task<CommentPostUserEntity?> GetById(string id);
    Task Delete(CommentPostUserEntity comment);
    Task<bool> ExistsById(string id);
    IQueryable<CommentPostUserEntity> Query();
    Task<CommentPostUserEntity> Update(CommentPostUserEntity comment, UpdateCommentPostUserDto dto);
}