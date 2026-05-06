using Domain_Service.RepoInterfaces.GenricRepo;
using Infrastructure_Service.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure_Service.Persistance.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<string> AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return "Data Has Been Added";
        }


        public async Task<string> UpdatedRangeAsync(List<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return "Data Has been Updated";
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> Remove(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _dbSet.Remove(entity);
            return true;
        }

        public async Task<T> Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

    }
}