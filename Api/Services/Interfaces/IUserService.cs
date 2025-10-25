using Api.models.dtos.Users;
using Api.models.entities;

namespace Api.Services.Interfaces;

public interface IUserService
{
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity?> GetUserByUsername(string username);
    Task<UserResult> CreateAsync(CreateUserDto dto);
    Task<UserResult> DeleteUser(UserEntity user);
    Task<UserResult> UpdateAsync(UserEntity user, UpdateUserDto dto);


}