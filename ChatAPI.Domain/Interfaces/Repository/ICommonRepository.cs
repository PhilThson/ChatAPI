using System.Linq.Expressions;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface ICommonRepository<T> where T : class
	{
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetByConditionAsync(
            Expression<Func<T, bool>> predicate, bool isTracked = false);
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);
        ValueTask<T?> FindByIdAsync(int id);
        public bool Exists(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

