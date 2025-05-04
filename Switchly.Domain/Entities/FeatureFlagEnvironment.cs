namespace Switchly.Domain.Entities;

public class FeatureFlagEnvironment
{
    public Guid Id { get; set; }

    public Guid FeatureFlagId { get; set; }
    public FeatureFlag FeatureFlag { get; set; } = null!;

    public Guid FlagEnvironmentId { get; set; }
    public FlagEnvironment FlagEnvironment { get; set; } = null!;

    public bool IsEnabled { get; set; } = false;
    public int RolloutPercentage { get; set; } = 100;
    public ICollection<FeatureFlagEnvironment> FeatureFlagEnvironments { get; set; } = new List<FeatureFlagEnvironment>();

}