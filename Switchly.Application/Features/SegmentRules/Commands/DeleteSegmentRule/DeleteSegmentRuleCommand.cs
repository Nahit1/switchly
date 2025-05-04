using MediatR;
using Switchly.Application.Common.Models;

namespace Switchly.Application.Features.SegmentRules.Commands.DeleteSegmentRule;

public record DeleteSegmentRuleCommand(Guid Id) : IRequest<ApiResponse<bool>>;