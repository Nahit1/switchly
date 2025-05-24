using MediatR;
using Switchly.Application.Common.Models;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;

namespace Switchly.Application.Features.SegmentRules.Commands.AddSegmentRule;

public class AddSegmentRuleCommandHandler : IRequestHandler<AddSegmentRuleCommand, ApiResponse<Guid>>
{
    private readonly ApplicationDbContext _dbContext;

    public AddSegmentRuleCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<Guid>> Handle(AddSegmentRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = new SegmentRule
        {
            Id = Guid.NewGuid(),
            FeatureFlagId = request.FeatureFlagId,
            Property = request.Property,
            Operator = request.Operator,
            Value = request.Value,
            RolloutPercentage = request.RolloutPercentage,
        };

        await _dbContext.SegmentRules.AddAsync(rule, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(rule.Id);
    }
}
