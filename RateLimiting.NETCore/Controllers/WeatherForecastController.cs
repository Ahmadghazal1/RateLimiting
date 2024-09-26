using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimiting.NETCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("FixedWindoPolicy")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("GetName")]
        [EnableRateLimiting("TokenBucketPolicy")]
        public IActionResult GetName()
        {
            var random = new Random();
            var index = random.Next(1, 5);
            List<string> students = new List<string> { "Ahmad", "Samer", "Ahmad", "Said", "Ghazal" };
            var name = students[index];
            return Ok(name);
        }
    }
}
