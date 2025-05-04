using MediatR;
using Switchly.Application.Common.Models;

namespace Switchly.Application.Features.SegmentRules.Commands.AddSegmentRule;

public record AddSegmentRuleCommand(
    Guid FeatureFlagId,
    string Property,
    string Operator,
    string Value,
    int RolloutPercentage = 100,
    int Order = 0
) : IRequest<ApiResponse<Guid>>;