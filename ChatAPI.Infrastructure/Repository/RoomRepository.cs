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

        public new Task<Room?> GetByIdAsync(int id) =>
            GetByConditionAsync(r => r.Id == id)
            .Include(r => r.Messages)
            .FirstOrDefaultAsync();
    }
}

