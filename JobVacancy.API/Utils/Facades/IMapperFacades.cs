using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Utils.Facades;

public interface IMapperFacades
{ 
    UserEntity Map(CreateUserDto dto);
}