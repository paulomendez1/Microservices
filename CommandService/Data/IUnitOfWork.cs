namespace CommandService.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICommandRepository CommandRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
