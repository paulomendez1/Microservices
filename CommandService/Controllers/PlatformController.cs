using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platform")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public PlatformController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlatformController> log)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            _log.LogInformation("Getting platforms...");

            var platformItems = await _unitOfWork.CommandRepository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }

        [HttpPost]
        public IActionResult TestInbounceConnection()
        {
            Console.WriteLine("--> Inbound Post # Command Service");
            return Ok("Inbound test from Platform Controller");
        }
    }
}
