using System.Linq.Expressions;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Infrastructure.Repository
{
    public class CommonRepository<T> : ICommonRepository<T>
        where T : class
	{
        private readonly ChatDbContext _context;
        private readonly DbSet<T> _dbSet;

		public CommonRepository(ChatDbContext dbContext)
		{
            _context = dbContext;
			_dbSet = dbContext.Set<T>();
		}

        public Task<List<T>> GetAllAsync() =>
            _dbSet.AsNoTracking().ToListAsync();

        public Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate) =>
            _dbSet.AsNoTracking().Where(predicate).ToListAsync();

        public Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate) =>
            _dbSet.FirstOrDefaultAsync(predicate);

        public ValueTask<T?> GetByIdAsync(int id) =>
            _dbSet.FindAsync(id);

        public bool Exists(Expression<Func<T, bool>> predicate) =>
            _dbSet.Any(predicate);

        public void Add(T entity) =>
            _dbSet.Add(entity);

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity) =>
            _dbSet.Remove(entity);
    }
}

