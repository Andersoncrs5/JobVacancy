using JobVacancy.API.models.dtos.Resume;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IResumeService
{
    Task<bool> ExistsByName(string name);
    Task<ResumeEntity?> GetByName(string name);
    Task<bool> ExistsByObjectKey(string key);
    Task<ResumeEntity?> GetByObjectKey(string key);
    Task<bool> ExistsByUrl(string url);
    Task<ResumeEntity?> GetByUrl(string url);
    IQueryable<ResumeEntity> Query();
    Task Delete(ResumeEntity entity);
    Task<ResumeEntity> Create(CreateResumeDto dto, string userId);
}