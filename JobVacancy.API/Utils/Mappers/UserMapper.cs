using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using AutoMapper;

namespace JobVacancy.API.Utils.Mappers;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserEntity, UserDto>();
    }
}