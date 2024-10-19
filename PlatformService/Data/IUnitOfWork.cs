namespace PlatformService.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IPlatformRepository PlatformRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
