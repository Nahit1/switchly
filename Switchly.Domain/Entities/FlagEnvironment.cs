namespace Switchly.Domain.Entities;

public class FlagEnvironment
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public string Name { get; set; } = null!; // Örnek: "prod", "staging", "dev"

    public ICollection<FeatureFlagEnvironment> FeatureFlagEnvironments { get; set; } = new List<FeatureFlagEnvironment>();
}