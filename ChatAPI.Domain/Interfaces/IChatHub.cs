using ChatAPI.Domain.DTOs;

namespace ChatAPI.Domain.Interfaces
{
    public interface IChatHub
	{
        Task SendMessage(MessageDto message);
    }
}

