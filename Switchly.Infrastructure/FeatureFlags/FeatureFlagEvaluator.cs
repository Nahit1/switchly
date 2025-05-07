using Microsoft.EntityFrameworkCore;
using Switchly.Persistence.Db;
using System.Security.Cryptography;
using System.Text;

public class FeatureFlagEvaluator : IFeatureFlagEvaluator
{
    private readonly ApplicationDbContext _dbContext;

    public FeatureFlagEvaluator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsEnabledAsync(string flagKey, UserContextModel user)
    {
        var flag = await _dbContext.FeatureFlags
            .Include(f => f.SegmentRules)
            .FirstOrDefaultAsync(f => f.Key == flagKey && !f.IsArchived);

        if (flag == null)
            return false;

        foreach (var rule in flag.SegmentRules.OrderBy(r => r.Order))
        {
            var match = rule.Property switch
            {
                "id" => Evaluate(rule.Operator, user.UserId.ToString(), rule.Value),
                "role" => Evaluate(rule.Operator, user.Role, rule.Value),
                "country" => Evaluate(rule.Operator, user.Country, rule.Value),
                _ => user.Traits.TryGetValue(rule.Property, out var val) &&
                     Evaluate(rule.Operator, val, rule.Value)
            };

            if (match)
            {
                if (rule.RolloutPercentage >= 100)
                    return true;

                var hash = ComputeDeterministicHash(user.UserId.ToString(), flag.Key);
                return hash % 100 < rule.RolloutPercentage;
            }
        }

        return false;
    }

    private bool Evaluate(string op, string input, string expected)
    {
        return op switch
        {
            "equals" => input == expected,
            "not_equals" => input != expected,
            "contains" => input.Contains(expected),
            "starts_with" => input.StartsWith(expected),
            "ends_with" => input.EndsWith(expected),
            _ => false
        };
    }

    private int ComputeDeterministicHash(string userId, string flagKey)
    {
        using var sha = SHA256.Create();
        var input = $"{userId}:{flagKey}";
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToInt32(hashBytes, 0) & int.MaxValue;
    }
}
