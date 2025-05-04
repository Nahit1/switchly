using MediatR;
using Switchly.Application.Common.Models;
using Switchly.Application.Features.SegmentRules.Dtos;

namespace Switchly.Application.Features.SegmentRules.Queries.GetSegmentRulesByFeatureId;

public record GetSegmentRulesByFeatureIdQuery(Guid FeatureFlagId)
    : IRequest<ApiResponse<List<SegmentRuleDto>>>;