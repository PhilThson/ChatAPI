using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Infrastructure.Repository
{
    public class RoomRepository : CommonRepository<Room>, IRoomRepository
    {
        public RoomRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Room?> GetByIdWithMessagesAsync(int id, bool isTracked = false) =>
            GetByConditionAsync(r => r.Id == id, isTracked)
            .Include(r => r.Messages)
            .FirstOrDefaultAsync();

        public Task<Room?> GetByIdAsync(int id, bool isTracked = false) =>
            GetByConditionAsync(r => r.Id == id, isTracked)
            .FirstOrDefaultAsync();
    }
}

