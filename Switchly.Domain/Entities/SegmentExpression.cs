namespace Switchly.Domain.Entities;

public class SegmentExpression
{
  public Guid Id { get; set; }

  public string? Operator { get; set; } // AND, OR, NOT gibi

  public Guid? SegmentRuleId { get; set; }
  public SegmentRule? SegmentRule { get; set; }

  public Guid? ParentExpressionId { get; set; }
  public SegmentExpression? ParentExpression { get; set; }
  public ICollection<SegmentExpression> Children { get; set; } = new List<SegmentExpression>();

  public Guid FeatureFlagId { get; set; }
  public FeatureFlag FeatureFlag { get; set; } = null!;
}
