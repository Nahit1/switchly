public class SegmentExpressionDto
{
  public Guid Id { get; set; }
  public string? Operator { get; set; }

  public SegmentRuleDto? Rule { get; set; }
  public List<SegmentExpressionDto> Children { get; set; } = new();
}

public class SegmentRuleDto
{
  public Guid Id { get; set; }
  public string Property { get; set; } = default!;
  public string Operator { get; set; } = default!;
  public string Value { get; set; } = default!;
  public int RolloutPercentage { get; set; }
}
