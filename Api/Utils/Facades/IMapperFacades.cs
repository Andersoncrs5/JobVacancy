using Api.models.dtos;
using Api.models.dtos.Users;
using Api.models.entities;

namespace Api.Utils.Facades;

public interface IMapperFacades
{ 
    UserEntity Map(CreateUserDto dto);
}