using Switchly.Application.FeatureFlags.Interfaces;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace Switchly.Application.FeatureFlags.Services;

public class FeatureFlagService : IFeatureFlagService
{
    private readonly ApplicationDbContext _dbContext;

    public FeatureFlagService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FeatureFlag> CreateFeatureFlagAsync(Guid organizationId, string key, string name, string description, CancellationToken cancellationToken)
    {
        // Aynı key zaten var mı kontrol et
        var exists = await _dbContext.FeatureFlags
            .AnyAsync(f => f.OrganizationId == organizationId && f.Key == key, cancellationToken);



        if (exists)
            throw new InvalidOperationException($"Feature flag with key '{key}' already exists.");

        var featureFlag = new FeatureFlag
        {
            OrganizationId = organizationId,
            Key = key,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsArchived = false,
            IsEnabled = true,
            Name = name

        };

        var data = await _dbContext.FeatureFlags.AddAsync(featureFlag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return featureFlag;
    }
}
