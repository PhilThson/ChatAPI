using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IRoomRepository : ICommonRepository<Room>
	{
		new Task<Room?> GetByIdAsync(int id);
	}
}

