public class UserSegmentContextModel
{
    public string Env { get; set; }
    public Dictionary<string, string> Traits { get; set; } = new();
}
