using MassTransit;
using StackExchange.Redis;
using Switchly.Application.Common.Interfaces;
using Switchly.Shared.Events;

namespace RedisWorker.Consumers;

public class FeatureFlagEvaluatedConsumer:IConsumer<FeatureFlagEvaluatedEvent>
{
  private readonly IDatabase _redis;
  private readonly ILogger<FeatureFlagEvaluatedConsumer> _logger;
  private readonly IRedisKeyProvider _keyProvider;

  public FeatureFlagEvaluatedConsumer(IConnectionMultiplexer redis, ILogger<FeatureFlagEvaluatedConsumer> logger, IRedisKeyProvider keyProvider)
  {
    _redis = redis.GetDatabase();
    _logger = logger;
    _keyProvider = keyProvider;
  }

  public async Task Consume(ConsumeContext<FeatureFlagEvaluatedEvent> context)
  {
    var msg = context.Message;

    foreach (var redisKey in msg.RedisKeys)
    {
      await _redis.StringSetAsync(
        redisKey,
        msg.IsEnabled.ToString().ToLower(),
        TimeSpan.FromHours(12)
      );

      Console.WriteLine($"> Redis'e yazıldı: {redisKey} = {msg.IsEnabled}");
    }

  }
}
