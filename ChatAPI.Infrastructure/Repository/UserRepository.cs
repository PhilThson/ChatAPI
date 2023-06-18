using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.DataAccess;

namespace ChatAPI.Infrastructure.Repository
{
    public class UserRepository : CommonRepository<User>
	{
		public UserRepository(ChatDbContext dbContext) : base(dbContext)
		{

		}


	}
}

