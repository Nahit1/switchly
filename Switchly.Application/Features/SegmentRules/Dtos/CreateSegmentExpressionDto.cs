namespace Switchly.Application.Features.SegmentRules.Dtos;

public class CreateSegmentExpressionDto
{
  public string? Operator { get; set; }
  public SegmentRuleDto? Rule { get; set; }
  public List<SegmentExpressionDto> Children { get; set; } = new();
}
