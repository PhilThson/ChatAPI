namespace ChatAPI.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        IParticipantRepository Participant { get; }
        IMessageRepository Message { get; }
        IRoomRepository Room { get; }

        void Save();
        Task SaveAsync();
    }
}