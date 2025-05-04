public interface IFeatureFlagEvaluator
{
    Task<bool> IsEnabledAsync(string flagKey, UserContext user);
}