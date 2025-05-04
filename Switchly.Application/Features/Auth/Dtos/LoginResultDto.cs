namespace Switchly.Application.Features.Auth.Dtos;
public class LoginResultDto
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
    public string Role { get; set; } = null!;
    public string Token { get; set; } = null!;
}