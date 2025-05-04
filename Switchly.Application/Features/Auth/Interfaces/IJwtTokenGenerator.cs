namespace Switchly.Application.Features.Auth.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, Guid organizationId, string role);
}