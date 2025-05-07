public class EvaluateFeatureFlagRequest
{
    public string FlagKey { get; set; } = null!;
    public UserContextModel UserContextModel { get; set; } = null!;
}
