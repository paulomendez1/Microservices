using CommandService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommandService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;
        public CommandRepository(AppDbContext context)
        {
            _context = context;   
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command is null || platformId <= 0)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command); ;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Platforms.Add(platform);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public async Task<IEnumerable<Command>> GetAllCommandsForPlatform(int platformId)
        {
            return await _context.Commands.Where(c => c.PlatformId == platformId).OrderBy(c=> c.Platform.Name).ToListAsync();
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }

        public async Task<Command?> GetCommand(int platformId, int commandId)
        {
            return await _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefaultAsync();
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }
    }
}
