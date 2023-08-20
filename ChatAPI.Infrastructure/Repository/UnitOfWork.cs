using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.DataAccess;

namespace ChatAPI.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _context;
        private bool _disposed;

        private ICommonRepository<Participant> _participant;
        private IMessageRepository _message;
        private ICommonRepository<Room> _room;

        public UnitOfWork(ChatDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public ICommonRepository<Participant> Participant =>
            _participant ??= new CommonRepository<Participant>(_context);

        public IMessageRepository Message =>
            _message ??= new MessageRepository(_context);

        public ICommonRepository<Room> Room =>
            _room ??= new CommonRepository<Room>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

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

