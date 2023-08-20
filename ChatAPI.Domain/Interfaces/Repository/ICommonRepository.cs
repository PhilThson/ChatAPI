using System.Linq.Expressions;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface ICommonRepository<T> where T : class
	{
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);
        ValueTask<T?> GetByIdAsync(int id);
        public bool Exists(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

