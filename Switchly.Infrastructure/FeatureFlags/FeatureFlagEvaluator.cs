using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Interfaces;
using Switchly.Application.Common.Messaging;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;
using Switchly.Shared.Events;

public class FeatureFlagEvaluator : IFeatureFlagEvaluator
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEvaluateEventPublisher _eventPublisher;
    private readonly IRedisKeyProvider _redisKeyProvider;

    public FeatureFlagEvaluator(
        ApplicationDbContext dbContext,
        IEvaluateEventPublisher eventPublisher,
        IRedisKeyProvider redisKeyProvider)
    {
        _dbContext = dbContext;
        _eventPublisher = eventPublisher;
        _redisKeyProvider = redisKeyProvider;
    }

    public async Task<bool> IsEnabledAsync(string flagKey, UserSegmentContextModel userSegmentContextModel)
    {
        var flag = await _dbContext.FeatureFlags
            .Include(f => f.SegmentRules)
            .Include(o=>o.Organization)
            .FirstOrDefaultAsync(f => f.Key == flagKey && !f.IsArchived);

        if (flag is null)
            return false;


        bool isEnabled = flag.IsEnabled;
        if (isEnabled)
        {
          var hasSegmentRules = flag.SegmentRules is { Count: > 0 };
          isEnabled = !hasSegmentRules
            ? flag.IsEnabled
            : EvaluateSegmentRules(flag, userSegmentContextModel);
        }
        // 🔑 Redis key her zaman SHA256 hash ile üretilecek
        var redisKey = _redisKeyProvider.GetHashedKey(flag.Organization.ClientKey, flag.Key, userSegmentContextModel);


        // 🚀 Event publish (tek key kullanıyoruz artık)
        var @event = new FeatureFlagEvaluatedEvent(
          flag.Id,
          flag.Key,
          userSegmentContextModel,
          isEnabled,
          DateTime.UtcNow,
          flag.Organization.ClientKey,
          redisKey // her zaman hashli
        );

        await _eventPublisher.PublishAsync(@event);

        return isEnabled;

    }




    private bool EvaluateSegmentRules(FeatureFlag flag, UserSegmentContextModel userSegmentContextModel)
    {
      var contextProperties = new List<(string Property, string Value)>();


      if (userSegmentContextModel.Traits != null)
      {
        foreach (var kvp in userSegmentContextModel.Traits)
        {
          contextProperties.Add((kvp.Key, kvp.Value));
        }
      }
      var segmentCount = flag.SegmentRules.Where(x => x.FeatureFlagId == flag.Id);
      if (contextProperties.Count() < segmentCount.Count()) return false;
      foreach (var prop in contextProperties)
      {
        if (!string.IsNullOrWhiteSpace(prop.Value))
        {
          // var segment = flag.SegmentRules
          //   .FirstOrDefault(c => c.Property.Equals(prop.Property, StringComparison.OrdinalIgnoreCase)&&
          //                        c.Value.Equals(prop.Value, StringComparison.OrdinalIgnoreCase));

          var segment = flag.SegmentRules
            .FirstOrDefault(c => c.Property.Equals(prop.Property, StringComparison.OrdinalIgnoreCase));

          if (segment is null) return false;
          if (string.IsNullOrWhiteSpace(segment.Value) || !Evaluate(segment.Operator, segment.Value, segment.Value))
          {
            return false; // kural eşleşmedi
          }

          var minRollout = flag.SegmentRules.Min(r => r.RolloutPercentage);

          if (minRollout >= 100)
            return true;

          var hash = ComputeDeterministicHash(prop.Value, flag.Key);
          return hash % 100 < minRollout;

        }

      }

      // foreach (var rule in flag.SegmentRules)
      // {
      //   var matchContext = contextProperties
      //     .FirstOrDefault(c => c.Property.Equals(rule.Property, StringComparison.OrdinalIgnoreCase));
      //
      //   if (string.IsNullOrWhiteSpace(matchContext.Value) || !Evaluate(rule.Operator, matchContext.Value, rule.Value))
      //   {
      //     return false; // kural eşleşmedi
      //   }
      // }

      return true; // tüm kurallar eşleşti
        // foreach (var rule in flag.SegmentRules.OrderBy(r => r.Order))
        // {
        //     var match = rule.Property switch
        //     {
        //         "id" => Evaluate(rule.Operator, user.UserId.ToString(), rule.Value),
        //         "role" => Evaluate(rule.Operator, user.Role, rule.Value),
        //         "country" => Evaluate(rule.Operator, user.Country, rule.Value),
        //         _ => user.Traits.TryGetValue(rule.Property, out var val) &&
        //              Evaluate(rule.Operator, val, rule.Value)
        //     };
        //
        //     if (!match)
        //     {
        //       return false; // 👈 bir tanesi bile eşleşmiyorsa feature disabled
        //     }
        // }

        // var minRollout = flag.SegmentRules.Min(r => r.RolloutPercentage);
        //
        // if (minRollout >= 100)
        //   return true;
        //
        // var hash = ComputeDeterministicHash(user.UserId.ToString(), flag.Key);
        // return hash % 100 < minRollout;
    }

    private bool Evaluate(string op, string input, string expected)
    {
      return op switch
      {
        "equals" => string.Equals(input, expected, StringComparison.OrdinalIgnoreCase),
        "not_equals" => !string.Equals(input, expected, StringComparison.OrdinalIgnoreCase),
        "contains" => input?.Contains(expected, StringComparison.OrdinalIgnoreCase) == true,
        "starts_with" => input?.StartsWith(expected, StringComparison.OrdinalIgnoreCase) == true,
        "ends_with" => input?.EndsWith(expected, StringComparison.OrdinalIgnoreCase) == true,
        "in" => expected
          ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
          .Contains(input, StringComparer.OrdinalIgnoreCase) == true,
        _ => false
      };
    }

    private int ComputeDeterministicHash(string val, string flagKey)
    {
        using var sha = SHA256.Create();
        var input = $"{val}:{flagKey}";
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToInt32(hashBytes, 0) & int.MaxValue;
    }
}
