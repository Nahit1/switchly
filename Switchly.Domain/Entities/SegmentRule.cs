namespace Switchly.Domain.Entities;

public class SegmentRule
{
    public Guid Id { get; set; }
    public Guid FeatureFlagId { get; set; }
    public FeatureFlag FeatureFlag { get; set; } = null!;

    public string Property { get; set; } = null!;    // user.role
    public string Operator { get; set; } = "equals"; // equals, contains, starts_with
    public string Value { get; set; } = null!;

    public int RolloutPercentage { get; set; } = 100;
    public int Order { get; set; } = 0;
}
