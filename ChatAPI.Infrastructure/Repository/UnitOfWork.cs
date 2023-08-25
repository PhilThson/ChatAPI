using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Infrastructure.DataAccess;

namespace ChatAPI.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _context;
        private bool _disposed;

        private IParticipantRepository _participant;
        private IMessageRepository _message;
        private IRoomRepository _room;

        public UnitOfWork(ChatDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IParticipantRepository Participant =>
            _participant ??= new ParticipantRepository(_context);

        public IMessageRepository Message =>
            _message ??= new MessageRepository(_context);

        public IRoomRepository Room =>
            _room ??= new RoomRepository(_context);

        public void Save() =>
            _context.SaveChanges();

        public Task SaveAsync() =>
            _context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}

