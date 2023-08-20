namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IRoomService
	{
		Task<IEnumerable<T>> GetAll<T>();
        Task<T> GetById<T>(int id, int userId);
    }
}