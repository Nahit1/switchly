using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Interfaces;
using Switchly.Application.Common.Models;
using Switchly.Application.FeatureFlags.Dtos;
using Switchly.Persistence.Db;

namespace Switchly.Application.FeatureFlags.Queries.GetAllFeatureFlags;

public class GetAllFeatureFlagsQueryHandler : IRequestHandler<GetAllFeatureFlagsQuery, ApiResponse<List<FeatureFlagDto>>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;

    public GetAllFeatureFlagsQueryHandler(ApplicationDbContext dbContext, IUserContext userContext)
    {
      _dbContext = dbContext;
      _userContext = userContext;
    }

    public async Task<ApiResponse<List<FeatureFlagDto>>> Handle(GetAllFeatureFlagsQuery request, CancellationToken cancellationToken)
    {
        var orgId = _userContext.OrganizationId;
        var flags = await _dbContext.FeatureFlags.Include(x=>x.SegmentRules)
            .Where(f => f.OrganizationId == orgId && !f.IsArchived)
            .Select(f => new FeatureFlagDto
            {
                Id = f.Id,
                Key = f.Key,
                Name = f.Name,
                Description = f.Description,
                IsEnabled = f.IsEnabled,
                CreatedAt = f.CreatedAt,
                FeatureSegments = f.SegmentRules.Select(item => new FeatureSegment
                {
                  Property = item.Property,
                  Value = item.Value,
                  Operator = item.Operator,
                  RolloutPercentage = item.RolloutPercentage,
                }).ToList()

            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<FeatureFlagDto>>.Ok(flags);
    }
}
