using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Infrastructure.Repository
{
    public class ParticipantRepository : CommonRepository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Participant>> GetRoomParticipants(
                int roomId, bool isTracked = false) =>
            GetByConditionAsync(p => p.RoomId == roomId && p.IsActive, isTracked)
            .ToListAsync();

        public Task<List<Participant>> GetAllRoomParticipants(
                int roomId, bool isTracked = false) =>
            GetByConditionAsync(p => p.RoomId == roomId, isTracked)
            .ToListAsync();

        public override void Delete(Participant participant) =>
            participant.IsActive = false;
    }
}

