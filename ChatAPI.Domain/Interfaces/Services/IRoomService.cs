using ChatAPI.Domain.DTOs.Update;

namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IRoomService
	{
		Task<IEnumerable<T>> GetAll<T>(string userName);
        Task<T> GetById<T>(int id, int userId);
        Task<T> Create<T>(string name, int userId);
        Task<T> UpdateName<T>(DictionaryDto<int> update, int userId);
        Task Delete(int roomId, int userId);
        Task Join(int roomId, int userId);
    }
}