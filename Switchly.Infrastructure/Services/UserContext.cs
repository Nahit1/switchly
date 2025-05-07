using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Switchly.Application.Common.Interfaces;

namespace Switchly.Infrastructure.Services;

public class UserContext : IUserContext
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserContext(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Guid UserId =>
    Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new UnauthorizedAccessException("UserId claim not found."));

  public Guid OrganizationId =>
    Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("organizationId")?.Value
               ?? throw new UnauthorizedAccessException("OrganizationId claim not found."));
}
