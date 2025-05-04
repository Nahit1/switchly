using MediatR;
using Switchly.Application.Common.Models;

namespace Switchly.Application.FeatureFlags.Commands.ArchiveFeatureFlag;

public record ArchiveFeatureFlagCommand(Guid FeatureFlagId)
    : IRequest<ApiResponse<bool>>;