namespace Switchly.Application.FeatureFlags.Dtos;

public class FeatureFlagDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
}