using MediatR;
using Switchly.Application.FeatureFlags.Interfaces;

namespace Switchly.Application.FeatureFlags.Commands.CreateFeatureFlag;

public class CreateFeatureFlagCommandHandler : IRequestHandler<CreateFeatureFlagCommand, Guid>
{
    private readonly IFeatureFlagService _featureFlagService;

    public CreateFeatureFlagCommandHandler(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }

    public async Task<Guid> Handle(CreateFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        var flag = await _featureFlagService.CreateFeatureFlagAsync(
            request.OrganizationId,
            request.Key,
            request.Name,
            request.Description,
            cancellationToken);

        return flag.Id;
    }
}
