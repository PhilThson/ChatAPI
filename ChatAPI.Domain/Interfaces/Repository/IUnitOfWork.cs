using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        ICommonRepository<User> User { get; }
        ICommonRepository<Message> Message { get; }
        ICommonRepository<Room> Room { get; }

        void Save();
    }
}