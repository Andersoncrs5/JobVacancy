using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IPostEnterpriseService
{
    Task<PostEnterpriseEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task Delete(PostEnterpriseEntity entity);
    Task<PostEnterpriseEntity> Create(CreatePostEnterpriseDto dto, string enterpriseId);
    IQueryable<PostEnterpriseEntity> Query();
    Task<PostEnterpriseEntity> Update(UpdatePostEnterpriseDto dto, PostEnterpriseEntity entity); 
}