using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IParticipantRepository : ICommonRepository<Participant>
	{
        Task<List<Participant>> GetRoomParticipants(int roomId, bool isTracked = false);
        Task<List<Participant>> GetAllRoomParticipants(int roomId, bool isTracked = false);
        new void Delete(Participant participant);
    }
}

