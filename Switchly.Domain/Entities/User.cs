namespace Switchly.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "Admin"; // Admin, Developer, Viewer

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
