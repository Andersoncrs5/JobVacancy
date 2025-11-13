using JobVacancy.API.models.dtos.ApplicationVacancy;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IApplicationVacancyService
{
    Task<bool> ExistsById(string id);
    Task<ApplicationVacancyEntity?> GetById(string id);
    Task<bool> ExistsByVacancyIdAndUserId(string vacancyId, string userId);
    Task<ApplicationVacancyEntity?> GetByVacancyIdAndUserId(string vacancyId, string userId);
    IQueryable<ApplicationVacancyEntity> Query();
    Task Delete(ApplicationVacancyEntity app);
    Task<ApplicationVacancyEntity> Create(CreateApplicationVacancyDto dto, string userId);
    Task<ApplicationVacancyEntity> Update(UpdateApplicationVacancyDto dto, ApplicationVacancyEntity app);
}