using MediatR;
using Switchly.Application.Common.Models;
using Switchly.Application.FeatureFlags.Dtos;

namespace Switchly.Application.FeatureFlags.Queries;

public record GetAllFeatureFlagsQuery(Guid OrganizationId) 
    : IRequest<ApiResponse<List<FeatureFlagDto>>>;
