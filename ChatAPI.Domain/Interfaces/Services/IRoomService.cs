namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IRoomService
	{
		Task<IEnumerable<T>> GetAll<T>();
	}
}