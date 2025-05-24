using MediatR;
using Switchly.Domain.Entities;
using Switchly.Persistence.Db;

public class CreateSegmentExpressionTreeCommandHandler : IRequestHandler<CreateSegmentExpressionTreeCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateSegmentExpressionTreeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSegmentExpressionTreeCommand request, CancellationToken cancellationToken)
    {
        var rootId = await CreateExpressionRecursiveAsync(request.Root, null, request.FeatureFlagId, cancellationToken);
        return rootId;
    }

    private async Task<Guid> CreateExpressionRecursiveAsync(SegmentExpressionDto dto, Guid? parentId, Guid featureFlagId, CancellationToken ct)
    {
        Guid? ruleId = null;

        // SegmentRule varsa ekle
        if (dto.Rule is not null)
        {
          var rule = new SegmentRule
          {
            Id = Guid.NewGuid(),
            FeatureFlagId = featureFlagId,
            Property = dto.Rule.Property,
            Operator = dto.Rule.Operator,
            Value = dto.Rule.Value,
            RolloutPercentage = dto.Rule.RolloutPercentage
          };

          await _context.SegmentRules.AddAsync(rule, ct);
          ruleId = rule.Id;
        }

        // SegmentExpression oluştur
        var expression = new SegmentExpression
        {
          Id = Guid.NewGuid(),
          Operator = dto.Operator,
          SegmentRuleId = ruleId,
          ParentExpressionId = parentId,
          FeatureFlagId = featureFlagId
        };

        await _context.SegmentExpressions.AddAsync(expression, ct);
        await _context.SaveChangesAsync(ct);

        // Alt expression'ları recursive olarak ekle
        foreach (var child in dto.Children)
        {
          await CreateExpressionRecursiveAsync(child, expression.Id, featureFlagId, ct);
        }

        return expression.Id;
    }
}
