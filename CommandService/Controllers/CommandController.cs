using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platform/{platformId}/command")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public CommandController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommandController> log)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommandsForPlatform(int platformId)
        {
            _log.LogInformation($"Getting commands for PlatformId ={platformId}...");

            if (!_unitOfWork.CommandRepository.PlatformExists(platformId))
            {
                return NotFound($"Platform with Id ={platformId} not found");
            }
            var commandItems = await _unitOfWork.CommandRepository.GetAllCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commandItems));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<IActionResult> GetCommandForPlatform(int platformId, int commandId)
        {
            _log.LogInformation($"Getting command by ID: {commandId} for Platform with ID: {platformId}...");

            var commandItem = await _unitOfWork.CommandRepository.GetCommand(platformId, commandId);

            if (!_unitOfWork.CommandRepository.PlatformExists(platformId))
            {
                return NotFound($"Platform with Id ={platformId} not found");
            }
            if (commandItem is not null)
            {
                return Ok(_mapper.Map<CommandReadDTO>(commandItem));
            }

            string notFoundResponse = $"Command with Id {commandId} not found.";
            _log.LogWarning(notFoundResponse);
            return NotFound(notFoundResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommandForPlatform(int platformId, CommandCreateDTO command)
        {
            _log.LogInformation($"Creating Command for Platform ID:{platformId}...");

            if(!_unitOfWork.CommandRepository.PlatformExists(platformId))
            {
                return NotFound($"Platform with Id ={platformId} not found");
            }
            var commandModel = _mapper.Map<Command>(command);

            _unitOfWork.CommandRepository.CreateCommand(platformId, commandModel);
            await _unitOfWork.SaveChangesAsync();

            var commandReadDTO = _mapper.Map<CommandReadDTO>(commandModel);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId= commandReadDTO.Id }, commandReadDTO);
        }
    }
}
