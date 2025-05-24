using MediatR;

public class CreateSegmentExpressionTreeCommand : IRequest<Guid>
{
  public Guid FeatureFlagId { get; set; }
  public SegmentExpressionDto Root { get; set; } = null!;
}
