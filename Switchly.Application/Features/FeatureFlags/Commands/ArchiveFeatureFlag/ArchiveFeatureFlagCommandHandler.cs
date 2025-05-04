using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Persistence.Db;

namespace Switchly.Application.FeatureFlags.Commands.ArchiveFeatureFlag;

public class ArchiveFeatureFlagCommandHandler : IRequestHandler<ArchiveFeatureFlagCommand, ApiResponse<bool>>
{
    private readonly ApplicationDbContext _dbContext;

    public ArchiveFeatureFlagCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<bool>> Handle(ArchiveFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        var flag = await _dbContext.FeatureFlags
            .FirstOrDefaultAsync(f => f.Id == request.FeatureFlagId, cancellationToken);

        if (flag is null)
            return ApiResponse<bool>.Fail("Feature flag bulunamadı.");

        flag.IsArchived = true;
        flag.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true);
    }
}