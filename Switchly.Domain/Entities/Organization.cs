namespace Switchly.Domain.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<FeatureFlag> FeatureFlags { get; set; } = new List<FeatureFlag>();
    public ICollection<FlagEnvironment> FlagEnvironments { get; set; } = new List<FlagEnvironment>();

}
