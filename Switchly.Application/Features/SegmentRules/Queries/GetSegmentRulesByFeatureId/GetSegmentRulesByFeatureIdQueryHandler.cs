using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Application.Features.SegmentRules.Dtos;
using Switchly.Persistence.Db;

namespace Switchly.Application.Features.SegmentRules.Queries.GetSegmentRulesByFeatureId;

public class GetSegmentRulesByFeatureIdQueryHandler
    : IRequestHandler<GetSegmentRulesByFeatureIdQuery, ApiResponse<List<SegmentRuleDto>>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetSegmentRulesByFeatureIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<List<SegmentRuleDto>>> Handle(
        GetSegmentRulesByFeatureIdQuery request,
        CancellationToken cancellationToken)
    {
        var rules = await _dbContext.SegmentRules
            .Where(r => r.FeatureFlagId == request.FeatureFlagId)
            .Select(r => new SegmentRuleDto
            {
                Id = r.Id,
                Property = r.Property,
                Operator = r.Operator,
                Value = r.Value,
                RolloutPercentage = r.RolloutPercentage,
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<SegmentRuleDto>>.Ok(rules);
    }
}
