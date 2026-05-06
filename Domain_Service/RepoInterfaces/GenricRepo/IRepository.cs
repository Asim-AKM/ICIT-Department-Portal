using System.Linq.Expressions;

namespace Domain_Service.RepoInterfaces.GenricRepo
{
    public interface IRepository<T>  where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        Task<T> CreateAsync(T entity);
        Task<T> Update(T entity);
        Task<string> AddRangeAsync(List<T> entities);
        Task<string> UpdatedRangeAsync(List<T> entities);
        Task<bool> Remove(Guid id);
    }
}
