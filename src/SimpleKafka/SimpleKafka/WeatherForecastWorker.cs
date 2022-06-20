using Confluent.Kafka;

using SimpleKafka.Configs;
using SimpleKafka.Controllers;

using System.Text.Json;
using System.Threading;

namespace SimpleKafka
{
    public class WeatherForecastWorker : BackgroundService
    {
        private readonly WeatherForecastConsumerConfig _config;

        public WeatherForecastWorker(WeatherForecastConsumerConfig config)
        {
            _config = config;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {

                consumer.Subscribe(new List<string> { "WeatherForecast" });
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult =await Task.Run(()=> consumer.Consume(stoppingToken));
                    var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(consumeResult.Message.Value);
                    if (weatherForecast is not null)
                    {
                        WeatherForecastController._forecasts.Add(weatherForecast);
                    }
                }
                consumer.Close();
            }
        }
    }
}
