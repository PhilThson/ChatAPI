using ChatAPI.Domain.DTOs.Update;

namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IRoomService
	{
		Task<IEnumerable<T>> GetAll<T>();
        Task<T> GetById<T>(int id, int userId);
        Task<T> Create<T>(string name, int userId);
        Task<T> UpdateName<T>(DictionaryDto<int> update, int userId);
    }
}