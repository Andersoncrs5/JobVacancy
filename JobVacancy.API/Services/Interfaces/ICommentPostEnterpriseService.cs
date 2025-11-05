using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface ICommentPostEnterpriseService
{
    Task<CommentPostEnterpriseEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task Delete(CommentPostEnterpriseEntity entity);
    Task<CommentPostEnterpriseEntity> Create(CreateCommentPostEnterpriseDto dto, string userId,string? parentId);
    IQueryable<CommentPostEnterpriseEntity> Query();
    Task<CommentPostEnterpriseEntity> Update(CommentPostEnterpriseEntity comment, UpdateCommentPostEnterpriseDto dto);
}