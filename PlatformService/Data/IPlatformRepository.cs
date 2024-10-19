using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepository
    {
        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task<Platform?> GetPlatformByID(int id);
        void CreatePlatform(Platform plat);

    }
}
