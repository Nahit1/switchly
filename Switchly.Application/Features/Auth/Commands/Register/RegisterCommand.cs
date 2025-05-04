using MediatR;
using Switchly.Application.Common.Models;
using Switchly.Application.Features.Auth.Dtos;

namespace Switchly.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    Guid OrganizationId,
    string Role // "Admin", "User", vb.
) : IRequest<ApiResponse<LoginResultDto>>;