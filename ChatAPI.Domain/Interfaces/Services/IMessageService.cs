using ChatAPI.Domain.DTOs.Create;

namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IMessageService
	{
		Task<T> GetById<T>(int id);
		Task<IEnumerable<T>> GetByRoomId<T>(int roomId);
		Task<T> Create<T>(CreateMessageDto messageDto, int userId);
	}
}

