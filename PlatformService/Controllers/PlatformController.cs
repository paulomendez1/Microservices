using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/platform")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _log;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlatformController> log, 
            ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _log = log;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            _log.LogInformation("Getting platforms from command service...");

            var platformItems = await _unitOfWork.PlatformRepository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            _log.LogInformation("Getting platform by ID...");

            var platformItem = await _unitOfWork.PlatformRepository.GetPlatformByID(id);

            if (platformItem is not null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
            }

            string notFoundResponse = $"Platform with Id {id} not found.";
            _log.LogWarning(notFoundResponse);
            return NotFound(notFoundResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatform(PlatformCreateDTO platform)
        {
            _log.LogInformation("Creating Platform...");
            var platformModel = _mapper.Map<Platform>(platform);

            _unitOfWork.PlatformRepository.CreatePlatform(platformModel);
            await _unitOfWork.SaveChangesAsync();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platformModel);

            //Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var platformPublishedDTO = _mapper.Map<PlatformPublishedDTO>(platformReadDTO);
                platformPublishedDTO.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok("Service is up and running!");
        }
    }
}
