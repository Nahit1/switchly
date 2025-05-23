using MediatR;
using Switchly.Application.Common.Models;

namespace Switchly.Application.FeatureFlags.Commands.UpdateFeatureFlag;

public record UpdateFeatureFlagCommand(
    Guid Id,
    string Description,
    bool IsEnabled
) : IRequest<ApiResponse<bool>>;
