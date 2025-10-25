using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IUserService
{
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity?> GetUserByRefreshToken(string refreshToken);
    Task<UserEntity?> GetUserBySid(string sid);
    Task<UserEntity?> GetUserByUsername(string username);
    Task<UserResult> CreateAsync(CreateUserDto dto);
    Task<UserResult> DeleteUser(UserEntity user);
    Task<UserResult> UpdateAsync(UserEntity user, UpdateUserDto dto);
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsByUsername(string username);
    Task<UserResult> AddRoleToUser(UserEntity user, RoleEntity role);
    Task<IList<string>> GetRolesAsync(UserEntity user);
    Task<UserResult> UpdateSimple(UserEntity user);
    Task<bool> CheckPassword(UserEntity user, string password);


}