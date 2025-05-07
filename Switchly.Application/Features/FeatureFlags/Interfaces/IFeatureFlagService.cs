using Switchly.Domain.Entities;

namespace Switchly.Application.FeatureFlags.Interfaces;

public interface IFeatureFlagService
{
    Task<FeatureFlag> CreateFeatureFlagAsync(Guid organizationId, string key, string name, string description, CancellationToken cancellationToken);
}
