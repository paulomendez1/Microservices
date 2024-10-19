using CommandService.Models;

namespace CommandService.SyncDataServices.grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}
