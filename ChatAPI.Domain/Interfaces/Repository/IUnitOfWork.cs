using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        IMessageRepository Message { get; }
        ICommonRepository<Room> Room { get; }

        void Save();
    }
}