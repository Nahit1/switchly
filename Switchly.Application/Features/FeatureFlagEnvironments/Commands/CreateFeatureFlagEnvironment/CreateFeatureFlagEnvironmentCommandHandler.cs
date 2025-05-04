using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.Application.Common.Models;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;

namespace Switchly.Application.Features.FeatureFlagEnvironments.Commands.CreateFeatureFlagEnvironment;

public class CreateFeatureFlagEnvironmentCommandHandler
    : IRequestHandler<CreateFeatureFlagEnvironmentCommand, ApiResponse<Guid>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateFeatureFlagEnvironmentCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateFeatureFlagEnvironmentCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.FeatureFlags
            .AnyAsync(x => x.Id == request.FeatureFlagId, cancellationToken);

        if (!exists)
            return ApiResponse<Guid>.Fail("FeatureFlag not found.");

        var envExists = await _dbContext.FlagEnvironments
            .AnyAsync(x => x.Id == request.FlagEnvironmentId, cancellationToken);

        if (!envExists)
            return ApiResponse<Guid>.Fail("FlagEnvironment not found.");

        var entity = new FeatureFlagEnvironment
        {
            Id = Guid.NewGuid(),
            FeatureFlagId = request.FeatureFlagId,
            FlagEnvironmentId = request.FlagEnvironmentId,
            IsEnabled = request.IsEnabled,
            RolloutPercentage = request.RolloutPercentage
        };

        await _dbContext.FeatureFlagEnvironments.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(entity.Id);
    }
}