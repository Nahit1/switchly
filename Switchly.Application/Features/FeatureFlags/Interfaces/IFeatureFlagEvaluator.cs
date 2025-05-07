public interface IFeatureFlagEvaluator
{
    Task<bool> IsEnabledAsync(string flagKey, UserContextModel user);
}
