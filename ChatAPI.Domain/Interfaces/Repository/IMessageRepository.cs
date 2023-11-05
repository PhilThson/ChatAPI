using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IMessageRepository : ICommonRepository<Message>
	{
        Task<Message?> GetById(long id);
        Task<List<Message>> GetByRoomId(int roomId);
    }
}

