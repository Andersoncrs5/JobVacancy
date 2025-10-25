namespace JobVacancy.API.Repositories.Interfaces;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync();
    IQueryable ReturnIQueryable();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}