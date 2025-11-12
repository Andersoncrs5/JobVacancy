using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IAreaService
{
    Task<AreaEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task<bool> ExistsName(string name);
    Task Delete(AreaEntity area);
    Task<AreaEntity> Create(CreateAreaDto dto);
    Task<AreaEntity> Update(UpdateAreaDto dto, AreaEntity area);
    IQueryable<AreaEntity> Query();
}