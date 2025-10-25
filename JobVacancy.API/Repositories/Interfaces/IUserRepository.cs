using JobVacancy.API.models.entities;
using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetById(string id);
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity?> GetByUsername(string username);
    Task<IdentityResult> Delete(UserEntity user);
    Task<IdentityResult> Insert(UserEntity user);
    Task<IdentityResult> Update(UserEntity user);
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsByUsername(string username);
    Task<IdentityResult> AddRoleToUser(UserEntity user, RoleEntity role);
    Task<IList<string>> GetRolesAsync(UserEntity user);

}