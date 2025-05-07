namespace Switchly.Application.Common.Interfaces;

public interface IUserContext
{
  Guid UserId { get; }
  Guid OrganizationId { get; }
}

