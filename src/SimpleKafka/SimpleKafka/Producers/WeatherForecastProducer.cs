using Confluent.Kafka;

using SimpleKafka.Configs;

using System;
using System.Text.Json;

namespace SimpleKafka.Producers
{
    public class WeatherForecastProducer
    {
        private readonly WeatherForecastProducerConfigs _config;

        public WeatherForecastProducer(WeatherForecastProducerConfigs config)
        {
            _config = config;
        }

        public async Task ProduceAsync()
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var forecast = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(Random.Shared.Next(1,100)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = SummaryProvider.Summaries[Random.Shared.Next(SummaryProvider.Summaries.Length)]
            };
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(forecast) };

            await producer.ProduceAsync("WeatherForecast", message);
        }
    }
}
