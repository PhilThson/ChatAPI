using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        ICommonRepository<Participant> Participant { get; }
        IMessageRepository Message { get; }
        IRoomRepository Room { get; }

        void Save();
        Task SaveAsync();
    }
}