using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IPositionService
{
    IQueryable<PositionEntity> Query();
    Task<PositionEntity> Update(UpdatePositionDto dto, PositionEntity position);
    Task<PositionEntity> Create(CreatePositionDto dto);
    Task Delete(PositionEntity position);
    Task<bool> ExistsById(string id);
    Task<bool> ExistsByName(string name);
    Task<PositionEntity?> GetById(string id);
}