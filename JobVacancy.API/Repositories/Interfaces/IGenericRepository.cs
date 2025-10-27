using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> ExistsById(string Id);
    IQueryable<T> ReturnIQueryable();
    Task<T?> GetByIdAsync(string id);
    Task<T> AddAsync(T entity);
    Task<T> Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}