using MediatR;
using Switchly.Application.Common.Models;

namespace Switchly.Application.Features.FeatureFlagEnvironments.Commands.CreateFeatureFlagEnvironment;

public record CreateFeatureFlagEnvironmentCommand(
    Guid FeatureFlagId,
    Guid FlagEnvironmentId,
    bool IsEnabled,
    int RolloutPercentage
) : IRequest<ApiResponse<Guid>>;