using MediatR;
using Switchly.Application.Common.Models;

namespace Switchly.Application.Features.Organizations.Commands.CreateOrganization;

public record CreateOrganizationCommand(string Name) : IRequest<ApiResponse<Guid>>;