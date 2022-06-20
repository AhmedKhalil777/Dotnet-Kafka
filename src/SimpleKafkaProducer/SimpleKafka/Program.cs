using Confluent.Kafka;

using SimpleKafka;
using SimpleKafka.Configs;
using SimpleKafka.Producers;

using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(x => new WeatherForecastProducerConfigs
{
    BootstrapServers = "127.0.0.1:9092",
    ClientId = Dns.GetHostName(),
});

builder.Services.AddSingleton(x => new WeatherForecastConsumerConfig
{
    BootstrapServers = "127.0.0.1:9092",
    GroupId = "WeatherForecasts",
    AutoOffsetReset = AutoOffsetReset.Earliest
});


builder.Services.AddTransient<WeatherForecastProducer>();
builder.Services.AddHostedService<WeatherForecastWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
