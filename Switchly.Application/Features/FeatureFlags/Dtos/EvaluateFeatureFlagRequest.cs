public class EvaluateFeatureFlagRequest
{
    public string FlagKey { get; set; } = null!;
    public UserContext UserContext { get; set; } = null!;
}