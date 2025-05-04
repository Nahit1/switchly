using MediatR;

namespace Switchly.Application.FeatureFlags.Commands.CreateFeatureFlag;

public record CreateFeatureFlagCommand(
    Guid OrganizationId,
    string Key,
    string Description
) : IRequest<Guid>; // Flag ID dönecek