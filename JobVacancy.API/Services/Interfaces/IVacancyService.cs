using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IVacancyService
{
    Task<VacancyEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task Delete(VacancyEntity entity);
    IQueryable<VacancyEntity> Query();
    Task<VacancyEntity> Create(CreateVacancyDto dto, string enterpriseId);
    Task<VacancyEntity> Update(UpdateVacancyDto dto, VacancyEntity vacancy);
}