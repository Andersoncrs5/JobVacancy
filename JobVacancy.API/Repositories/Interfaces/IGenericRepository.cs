using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> ExistsById(string Id);
    Task<int> CountByIdAsync(string Id);
    IQueryable<T> ReturnIQueryable();
    Task<T?> GetByIdAsync(string id);
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<T> Update(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    void Delete(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task SaveChangesAsync();
    Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    
}