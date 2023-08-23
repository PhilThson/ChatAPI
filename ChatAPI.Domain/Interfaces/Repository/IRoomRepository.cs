using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IRoomRepository : ICommonRepository<Room>
	{
		Task<Room?> GetByIdAsync(int id, bool isTracked = false);
	}
}

