using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Infrastructure.Repository
{
    public class MessageRepository : CommonRepository<Message>, IMessageRepository
	{
		public MessageRepository(ChatDbContext dbContext) : base(dbContext)
		{
		}

        public Task<Message?> GetById(long id) =>
			_dbSet
				.AsNoTracking()
				.Include(m => m.Sender)
				.FirstOrDefaultAsync(m => m.Id == id);

        public Task<List<Message>> GetByRoomId(int roomId) =>
			_dbSet
				.AsNoTracking()
				.Where(m => m.Sender.RoomId == roomId)
				.Include(m => m.Sender)
				.ToListAsync();
	}
}

