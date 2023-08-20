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
        protected readonly DbSet<T> _dbSet;

		public CommonRepository(ChatDbContext dbContext)
		{
            _context = dbContext;
			_dbSet = dbContext.Set<T>();
		}

        public virtual Task<List<T>> GetAllAsync() =>
            _dbSet.AsNoTracking().ToListAsync();

        public virtual IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> predicate) =>
            _dbSet.AsNoTracking().Where(predicate);

        public virtual Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate) =>
            _dbSet.FirstOrDefaultAsync(predicate);

        public virtual ValueTask<T?> GetByIdAsync(int id) =>
            _dbSet.FindAsync(id);

        public bool Exists(Expression<Func<T, bool>> predicate) =>
            _dbSet.Any(predicate);

        public virtual void Add(T entity) =>
            _dbSet.Add(entity);

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity) =>
            _dbSet.Remove(entity);
    }
}

