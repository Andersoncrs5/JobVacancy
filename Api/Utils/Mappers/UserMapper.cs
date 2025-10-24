using Api.models.dtos;
using Api.models.dtos.Users;
using Api.models.entities;
using AutoMapper;

namespace Api.Utils.Mappers;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<CreateUserDto, UserEntity>();
    }
}