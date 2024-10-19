using CommandService.Models;
using CommandService.SyncDataServices.grpc;

namespace CommandService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<IUnitOfWork>(), platforms);
            }
        }

        private async static void SeedData(IUnitOfWork unitOfWork, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var plat in platforms)
            {
                if(!unitOfWork.CommandRepository.ExternalPlatformExists(plat.ExternalId))
                {
                    unitOfWork.CommandRepository.CreatePlatform(plat);
                }
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
