public class EvaluateFeatureFlagRequest
{
    public string FlagKey { get; set; } = null!;
    public string Env { get; set; }
    public UserSegmentContextModel UserContextModel { get; set; } = null!;
}
