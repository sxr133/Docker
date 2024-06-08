using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace SportingStats.Controllers.NBA_API
{
    [ApiController]
    [Route("api/[controller]")]
    public class NBAConferenceController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public NBAConferenceController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetNBAConference()
        {
            try
            {
                Console.WriteLine("inside the NBAConference API");
                string apiKey = _configuration["AppSettings:ApiKey"];
                var client = _clientFactory.CreateClient();
                var uri = new Uri("https://tank01-fantasy-stats.p.rapidapi.com/getNBATeams?teamStats=true");
                Console.WriteLine("Uri {0}", uri);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = uri,
                    Headers =
                        {
                            { "X-RapidAPI-Key", apiKey },
                            { "X-RapidAPI-Host", "tank01-fantasy-stats.p.rapidapi.com" },
                        },
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    // Parse the JSON string into a JObject
                    var jsonObject = JObject.Parse(body);

                    return Ok(jsonObject.ToString()); // Directly return the JSON object if the API's response is suitable


                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}