using PlatformService.DTOs;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDTO plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");

            string targetUrl = $"{_configuration["CommandService"]}";
            Console.WriteLine($"Target URL: {targetUrl}");
            var response = await _http.PostAsync(targetUrl, httpContent);
        
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService had an error");
            }
        }
    }
}
