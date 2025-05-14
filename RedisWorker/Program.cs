using MassTransit;
using RedisWorker;
using StackExchange.Redis;
using RedisWorker.Consumers;
using Switchly.Application.Common.Interfaces;

var host = Host.CreateDefaultBuilder(args)
  .ConfigureServices((context, services) =>
  {
    services.AddSingleton<IRedisKeyProvider, RedisKeyProvider>();

    // Redis bağlantısı
    services.AddSingleton<IConnectionMultiplexer>(
      ConnectionMultiplexer.Connect("localhost,allowAdmin=true")); // Docker içindeysen "redis" olabilir

    // MassTransit + Consumer tanımı
    services.AddMassTransit(x =>
    {
      x.AddConsumer<FeatureFlagEvaluatedConsumer>(); // 🟢 Consumer'ı bildir

      x.UsingRabbitMq((ctx, cfg) =>
      {
        cfg.Host("rabbitmq://localhost", h =>
        {
          h.Username("guest");
          h.Password("guest");
        });

        // 🟢 Exchange'e bağlanan queue tanımı
        cfg.ReceiveEndpoint("feature-flag-evaluated", e =>
        {
          e.ConfigureConsumer<FeatureFlagEvaluatedConsumer>(ctx); // 🟢 Mesajı consumer'a yönlendir
        });
      });
    });

    // Varsa Worker.cs için servis tanımı (zorunlu değil)
    services.AddHostedService<Worker>();
  })
  .Build();

await host.RunAsync();
