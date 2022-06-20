using Microsoft.AspNetCore.Mvc;

using SimpleKafka.Producers;

namespace SimpleKafka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastProducer _producer;
        public static  List<WeatherForecast> _forecasts;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastProducer producer)
        {
            _logger = logger;
            _producer = producer;
            if (_forecasts is null || !_forecasts.Any())
            {
                _forecasts =  Enumerable.Range(1, 3).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = SummaryProvider.Summaries[Random.Shared.Next(SummaryProvider.Summaries.Length)]
                })
                .ToList();
            }
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _forecasts;
        }

        [HttpPost(Name = "AddWeatherForecast")]
        public async Task<IActionResult> Post()
        {
            await _producer.ProduceAsync();
            return Ok(new { Status = "Ok" });
        }
    }
}