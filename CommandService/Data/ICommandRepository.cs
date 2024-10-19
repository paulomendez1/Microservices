using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepository
    {
        //Platforms
        Task<IEnumerable<Platform>> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        //Commands
        Task<IEnumerable<Command>> GetAllCommandsForPlatform(int platformId);
        Task<Command?> GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);

    }
}
