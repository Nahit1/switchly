using MediatR;
using Switchly.Application.Common.Models;
using Switchly.Application.Features.Auth.Dtos;

namespace Switchly.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<ApiResponse<LoginResultDto>>;