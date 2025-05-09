public class EvaluateFeatureFlagRequest
{
    public string FlagKey { get; set; } = null!;
    public UserSegmentContextModel UserContextModel { get; set; } = null!;
}
