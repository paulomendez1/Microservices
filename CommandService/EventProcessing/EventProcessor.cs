using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper; 
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event...");

            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);

            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("-->Platform Published Event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("-->Could not determine Event type");
                    return EventType.undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDTO);

                    if (!repo.CommandRepository.ExternalPlatformExists(platform.ExternalId))
                    {
                        repo.CommandRepository.CreatePlatform(platform);
                        repo.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add platform to db: {ex}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        undetermined
    }
}
