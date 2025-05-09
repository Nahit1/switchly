public class UserSegmentContextModel
{
    // public Guid? UserId { get; set; }
    // public string? Role { get; set; } = null!;
    // public string? Country { get; set; } = null!;
    public Dictionary<string, string> Traits { get; set; } = new();
}
