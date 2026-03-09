using Domain_Service.RepoInterfaces.GenricRepo;
using Infrastructure_Service.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure_Service.Persistance.GenericRepository
{
    public class Repository<T>(ApplicationDbContext dbContext) : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task<string> AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return "Data Has Been Added";
        }


        public  async Task<string> UpdatedRangeAsync(List<T> entities)
        {
             _dbSet.UpdateRange(entities);
            return "Data Has been Updated";
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
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
