namespace Switchly.Domain.Entities;

public class FeatureFlag
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsArchived { get; set; } = false;
    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<FeatureFlagEnvironment> FeatureFlagEnvironments { get; set; } = new List<FeatureFlagEnvironment>();
    public ICollection<SegmentRule> SegmentRules { get; set; } = new List<SegmentRule>();
    public ICollection<SegmentExpression> SegmentExpressions { get; set; } = new List<SegmentExpression>();



}
