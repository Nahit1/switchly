public interface IFeatureFlagEvaluator
{
    Task<bool> IsEnabledAsync(string flagKey, UserSegmentContextModel userSegmentContextModel);

}
