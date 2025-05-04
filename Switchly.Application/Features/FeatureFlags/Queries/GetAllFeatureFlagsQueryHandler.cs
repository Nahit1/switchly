using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Application.FeatureFlags.Dtos;
using Switchly.Persistence.Db;

namespace Switchly.Application.FeatureFlags.Queries.GetAllFeatureFlags;

public class GetAllFeatureFlagsQueryHandler : IRequestHandler<GetAllFeatureFlagsQuery, ApiResponse<List<FeatureFlagDto>>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllFeatureFlagsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<List<FeatureFlagDto>>> Handle(GetAllFeatureFlagsQuery request, CancellationToken cancellationToken)
    {
        var flags = await _dbContext.FeatureFlags
            .Where(f => f.OrganizationId == request.OrganizationId && !f.IsArchived)
            .Select(f => new FeatureFlagDto
            {
                Id = f.Id,
                Key = f.Key,
                Description = f.Description,
                IsArchived = f.IsArchived,
                CreatedAt = f.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<FeatureFlagDto>>.Ok(flags);
    }
}