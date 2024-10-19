
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly CommandRepository _commandRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _commandRepository = new CommandRepository(_context);
        }
        public ICommandRepository CommandRepository => _commandRepository;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
