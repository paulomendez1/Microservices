
namespace PlatformService.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly PlatformRepository _platformRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _platformRepository = new PlatformRepository(_context);
        }

        public IPlatformRepository PlatformRepository => _platformRepository;
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
