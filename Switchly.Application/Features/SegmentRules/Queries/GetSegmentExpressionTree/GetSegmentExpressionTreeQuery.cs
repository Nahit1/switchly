using MediatR;

namespace Switchly.Application.Features.SegmentRules.Queries.GetSegmentExpressionTree;

public record GetSegmentExpressionTreeQuery(Guid FeatureFlagId) : IRequest<SegmentExpressionDto>;

