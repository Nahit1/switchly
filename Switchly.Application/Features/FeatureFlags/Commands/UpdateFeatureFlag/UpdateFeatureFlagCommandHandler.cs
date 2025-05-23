using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Persistence.Db;

namespace Switchly.Application.FeatureFlags.Commands.UpdateFeatureFlag;

public class UpdateFeatureFlagCommandHandler : IRequestHandler<UpdateFeatureFlagCommand, ApiResponse<bool>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateFeatureFlagCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        var flag = await _dbContext.FeatureFlags
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (flag is null)
            return ApiResponse<bool>.Fail("Feature flag bulunamadı.");

        flag.IsEnabled = request.IsEnabled;

        flag.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true);
    }
}
