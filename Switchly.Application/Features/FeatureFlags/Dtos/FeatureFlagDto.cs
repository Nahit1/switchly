namespace Switchly.Application.FeatureFlags.Dtos;

public class FeatureFlagDto
{
    public FeatureFlagDto()
    {
      FeatureSegments = new();
    }
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Key { get; set; } = null!;
    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<FeatureSegment> FeatureSegments { get; set; }
}


public class FeatureSegment
{
  public Guid Id { get; set; }
  public string Property { get; set; } = null!;    // user.role
  public string Operator { get; set; } = "equals"; // equals, contains, starts_with
  public string Value { get; set; } = null!;

  public int RolloutPercentage { get; set; } = 100;

}
