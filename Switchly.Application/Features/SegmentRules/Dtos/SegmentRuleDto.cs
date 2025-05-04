namespace Switchly.Application.Features.SegmentRules.Dtos;

public class SegmentRuleDto
{
    public Guid Id { get; set; }
    public string Property { get; set; } = null!;
    public string Operator { get; set; } = null!;
    public string Value { get; set; } = null!;
    public int RolloutPercentage { get; set; }
    public int Order { get; set; }
}